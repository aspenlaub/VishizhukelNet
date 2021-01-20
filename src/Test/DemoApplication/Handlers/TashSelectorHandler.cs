using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class TashSelectorHandler : TashSelectorHandlerBase<IDemoApplicationModel> {
        private readonly IDemoHandlers vDemoApplicationHandlers;

        public TashSelectorHandler(IDemoHandlers demoApplicationHandlers, ISimpleLogger simpleLogger, ITashCommunicator<IDemoApplicationModel> tashCommunicator, Dictionary<string, ISelector> selectors)
            : base(simpleLogger, tashCommunicator, selectors) {
            vDemoApplicationHandlers = demoApplicationHandlers;
        }

        protected override async Task SelectedIndexChangedAsync(ITashTaskHandlingStatus<IDemoApplicationModel> status, string controlName, int selectedIndex, bool selectablesChanged) {
            if (selectedIndex >= 0) {
                SimpleLogger.LogInformation($"Changing selected index for {controlName} to {selectedIndex}");
                switch (controlName) {
                    case nameof(status.Model.Beta):
                        await vDemoApplicationHandlers.BetaSelectorHandler.SelectedIndexChangedAsync(selectedIndex);
                        break;
                    default:
                        var errorMessage = $"Do not know how to select for {status.TaskBeingProcessed.ControlName}";
                        SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
                        await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                        break;
                }
            }
        }

        public override async Task ProcessSelectComboOrResetTaskAsync(ITashTaskHandlingStatus<IDemoApplicationModel> status) {
            if (!Selectors.ContainsKey(status.TaskBeingProcessed.ControlName)) {
                var errorMessage = $"Unknown selector control {status.TaskBeingProcessed.ControlName}";
                SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
                await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                return;
            }

            SimpleLogger.LogInformation($"{status.TaskBeingProcessed.ControlName} is a valid selector");
            var selector = Selectors[status.TaskBeingProcessed.ControlName];

            var controlName = status.TaskBeingProcessed.ControlName;
            await SelectedIndexChangedAsync(status, controlName, 0, false);
            if (status.TaskBeingProcessed.Status == ControllableProcessTaskStatus.BadRequest) { return; }

            var itemToSelect = status.TaskBeingProcessed.Text;
            await SelectItemAsync(status, selector, itemToSelect, controlName);
        }
    }
}
