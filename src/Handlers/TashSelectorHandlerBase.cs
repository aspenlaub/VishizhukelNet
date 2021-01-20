using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers {
    public abstract class TashSelectorHandlerBase<TModel> : ITashSelectorHandler<TModel> where TModel : IApplicationModel {
        protected readonly ISimpleLogger SimpleLogger;
        protected readonly ITashCommunicator<TModel> TashCommunicator;
        protected readonly Dictionary<string, ISelector> Selectors;

        protected TashSelectorHandlerBase(ISimpleLogger simpleLogger, ITashCommunicator<TModel> tashCommunicator, Dictionary<string, ISelector> selectors) {
            SimpleLogger = simpleLogger;
            TashCommunicator = tashCommunicator;
            Selectors = selectors;
        }

        protected abstract Task SelectedIndexChangedAsync(ITashTaskHandlingStatus<TModel> status, string controlName, int selectedIndex, bool selectablesChanged);
        public abstract Task ProcessSelectComboOrResetTaskAsync(ITashTaskHandlingStatus<TModel> status);
        protected virtual void OnItemAlreadySelected(ITashTaskHandlingStatus<TModel> status) { }

        protected async Task SelectItemAsync(ITashTaskHandlingStatus<TModel> status, ISelector selector, string itemToSelect, string controlName) {
            var selectedItemName = selector?.SelectedItem?.Name ?? "";
            if (selectedItemName == itemToSelect) {
                SimpleLogger.LogInformation($"{controlName} already is set to {itemToSelect}");
                OnItemAlreadySelected(status);
                await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
            } else {
                var selectedIndex = selector?.Selectables.FindIndex(s => s.Name == itemToSelect) ?? -1;
                if (selectedIndex < 0) {
                    var errorMessage = $"Unknown item {itemToSelect} for {controlName}";
                    SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
                    await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                } else {
                    SimpleLogger.LogInformation($"Found \"{itemToSelect}\" at index {selectedIndex}");
                    await SelectedIndexChangedAsync(status, controlName, selectedIndex, true);
                    if (status.TaskBeingProcessed.Status == ControllableProcessTaskStatus.BadRequest) {
                        return;
                    }

                    selectedItemName = selector?.SelectedItem?.Name ?? "";
                    if (selectedItemName == itemToSelect) {
                        SimpleLogger.LogInformation($"\"{itemToSelect}\" selected for {controlName}");
                        await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
                    } else {
                        var errorMessage = $"Could not select \"{itemToSelect}\" for {controlName}, it is \"{selectedItemName}\"";
                        SimpleLogger.LogInformation($"Communicating 'Failed' to remote controlling process ({errorMessage})");
                        await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.Failed, false, "", errorMessage);
                    }
                }
            }
        }
    }
}
