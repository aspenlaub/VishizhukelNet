using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;

public abstract class TashSelectorHandlerBase<TModel> : ITashSelectorHandler<TModel> where TModel : IApplicationModelBase {
    protected readonly ISimpleLogger SimpleLogger;
    protected readonly ITashCommunicator<TModel> TashCommunicator;
    protected readonly Dictionary<string, ISelector> Selectors;
    protected readonly IMethodNamesFromStackFramesExtractor MethodNamesFromStackFramesExtractor;

    protected TashSelectorHandlerBase(ISimpleLogger simpleLogger, ITashCommunicator<TModel> tashCommunicator, Dictionary<string, ISelector> selectors, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor) {
        SimpleLogger = simpleLogger ?? throw new ArgumentNullException(nameof(simpleLogger));
        TashCommunicator = tashCommunicator ?? throw new ArgumentNullException(nameof(tashCommunicator));
        Selectors = selectors ?? throw new ArgumentNullException(nameof(selectors));
        MethodNamesFromStackFramesExtractor = methodNamesFromStackFramesExtractor ?? throw new ArgumentNullException(nameof(methodNamesFromStackFramesExtractor));
    }

    protected abstract Task SelectedIndexChangedAsync(ITashTaskHandlingStatus<TModel> status, string controlName, int selectedIndex, bool selectablesChanged);
    public abstract Task ProcessSelectComboOrResetTaskAsync(ITashTaskHandlingStatus<TModel> status);
    protected virtual void OnItemAlreadySelected(ITashTaskHandlingStatus<TModel> status) { }

    protected async Task SelectItemAsync(ITashTaskHandlingStatus<TModel> status, ISelector selector, string itemToSelect, string controlName) {
        var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
        var selectedItemName = selector?.SelectedItem?.Name ?? "";
        if (selectedItemName == itemToSelect) {
            SimpleLogger.LogInformationWithCallStack($"{controlName} already is set to {itemToSelect}", methodNamesFromStack);
            OnItemAlreadySelected(status);
            await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
        } else {
            var selectedIndex = selector?.Selectables.FindIndex(s => s.Name == itemToSelect) ?? -1;
            if (selectedIndex < 0) {
                var errorMessage = $"Unknown item {itemToSelect} for {controlName}";
                SimpleLogger.LogInformationWithCallStack($"Communicating 'BadRequest' to remote controlling process ({errorMessage})", methodNamesFromStack);
                await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
            } else {
                SimpleLogger.LogInformationWithCallStack($"Found \"{itemToSelect}\" at index {selectedIndex}", methodNamesFromStack);
                await SelectedIndexChangedAsync(status, controlName, selectedIndex, true);
                if (status.TaskBeingProcessed.Status == ControllableProcessTaskStatus.BadRequest) {
                    return;
                }

                selectedItemName = selector?.SelectedItem?.Name ?? "";
                if (selectedItemName == itemToSelect) {
                    SimpleLogger.LogInformationWithCallStack($"\"{itemToSelect}\" selected for {controlName}", methodNamesFromStack);
                    await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
                } else {
                    var errorMessage = $"Could not select \"{itemToSelect}\" for {controlName}, it is \"{selectedItemName}\"";
                    SimpleLogger.LogInformationWithCallStack($"Communicating 'Failed' to remote controlling process ({errorMessage})", methodNamesFromStack);
                    await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.Failed, false, "", errorMessage);
                }
            }
        }
    }
}