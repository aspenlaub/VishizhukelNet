using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;

public class TashSelectorHandler : TashSelectorHandlerBase<IApplicationModel> {
    private readonly IApplicationHandlers _DemoApplicationHandlers;

    public TashSelectorHandler(IApplicationHandlers demoApplicationHandlers, ISimpleLogger simpleLogger, ITashCommunicator<IApplicationModel> tashCommunicator, Dictionary<string, ISelector> selectors, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor)
        : base(simpleLogger, tashCommunicator, selectors, methodNamesFromStackFramesExtractor) {
        _DemoApplicationHandlers = demoApplicationHandlers;
    }

    protected override async Task SelectedIndexChangedAsync(ITashTaskHandlingStatus<IApplicationModel> status, string controlName, int selectedIndex, bool selectablesChanged) {
        var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
        if (selectedIndex >= 0) {
            SimpleLogger.LogInformationWithCallStack($"Changing selected index for {controlName} to {selectedIndex}", methodNamesFromStack);
            switch (controlName) {
                case nameof(status.Model.Beta):
                    await _DemoApplicationHandlers.BetaSelectorHandler.SelectedIndexChangedAsync(selectedIndex);
                    break;
                default:
                    var errorMessage = $"Do not know how to select for {status.TaskBeingProcessed.ControlName}";
                    SimpleLogger.LogInformationWithCallStack($"Communicating 'BadRequest' to remote controlling process ({errorMessage})", methodNamesFromStack);
                    await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                    break;
            }
        }
    }

    public override async Task ProcessSelectComboOrResetTaskAsync(ITashTaskHandlingStatus<IApplicationModel> status) {
        var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
        var controlName = status.TaskBeingProcessed.ControlName;
        if (!Selectors.ContainsKey(controlName)) {
            var errorMessage = $"Unknown selector control {controlName}";
            SimpleLogger.LogInformationWithCallStack($"Communicating 'BadRequest' to remote controlling process ({errorMessage})", methodNamesFromStack);
            await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
            return;
        }

        SimpleLogger.LogInformationWithCallStack($"{controlName} is a valid selector", methodNamesFromStack);
        var selector = Selectors[controlName];

        await SelectedIndexChangedAsync(status, controlName, -1, false);
        if (status.TaskBeingProcessed.Status == ControllableProcessTaskStatus.BadRequest) { return; }

        var itemToSelect = status.TaskBeingProcessed.Text;
        await SelectItemAsync(status, selector, itemToSelect, controlName);
    }
}