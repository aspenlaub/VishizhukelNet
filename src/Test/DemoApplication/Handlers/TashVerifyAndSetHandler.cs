using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class TashVerifyAndSetHandler : TashVerifyAndSetHandlerBase<IDemoApplicationModel> {
        private readonly IDemoHandlers vDemoApplicationHandlers;

        public TashVerifyAndSetHandler(IDemoHandlers demoApplicationHandlers, ISimpleLogger simpleLogger, ITashSelectorHandler<IDemoApplicationModel> tashSelectorHandler,
            ITashCommunicator<IDemoApplicationModel> tashCommunicator, Dictionary<string, ISelector> selectors)
            : base(simpleLogger, tashSelectorHandler, tashCommunicator, selectors) {
            vDemoApplicationHandlers = demoApplicationHandlers;
        }

        public override async Task ProcessVerifyWhetherEnabledTaskAsync(ITashTaskHandlingStatus<IDemoApplicationModel> status) {
            bool actualEnabled;
            switch (status.TaskBeingProcessed.ControlName) {
                case nameof(status.Model.Alpha):
                    actualEnabled = status.Model.Alpha.Enabled;
                    break;
                case nameof(status.Model.Beta):
                    actualEnabled = status.Model.Beta.Selectables.ToList().Any();
                    break;
                default:
                    var errorMessage = $"Unknown enabled/disabled control {status.TaskBeingProcessed.ControlName}";
                    SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
                    await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                    return;
            }

            status.Model.Status.Type = StatusType.Success;
            if (status.TaskBeingProcessed.Text == "true") {
                if (!actualEnabled) {
                    status.Model.Status.Type = StatusType.Error;
                    status.Model.Status.Text = $"Expected {status.TaskBeingProcessed.ControlName} to be enabled";
                }
            } else if (actualEnabled) {
                status.Model.Status.Type = StatusType.Error;
                status.Model.Status.Text = $"Expected {status.TaskBeingProcessed.ControlName} to be disabled";
            }

            await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
        }

        public override async Task ProcessVerifyNumberOfItemsTaskAsync(ITashTaskHandlingStatus<IDemoApplicationModel> status) {
            if (string.IsNullOrWhiteSpace(status.TaskBeingProcessed.Text)) {
                var errorMessage = $"No number of items specified for {status.TaskBeingProcessed.ControlName}";
                SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process {errorMessage}");
                await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                return;
            }

            if (!Selectors.ContainsKey(status.TaskBeingProcessed.ControlName)) {
                var errorMessage = $"Unknown control {status.TaskBeingProcessed.ControlName} with number of items";
                SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
                await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                return;
            }
            var actualNumberOfItems = Selectors[status.TaskBeingProcessed.ControlName].Selectables.Count;
            status.Model.Status.Text = actualNumberOfItems.ToString() == status.TaskBeingProcessed.Text
                ? ""
                : $"Expected {status.TaskBeingProcessed.Text} item/-s on {status.TaskBeingProcessed.ControlName}, got {actualNumberOfItems}";
            status.Model.Status.Type = string.IsNullOrEmpty(status.Model.Status.Text) ? StatusType.Success : StatusType.Error;
            await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
        }

        protected override Dictionary<string, ITextBox> TextBoxNamesToTextBoxDictionary(ITashTaskHandlingStatus<IDemoApplicationModel> status) {
            return new Dictionary<string, ITextBox> {
                { nameof(status.Model.Alpha), status.Model.Alpha }
            };
        }

        protected override Dictionary<string, ISimpleTextHandler> TextBoxNamesToTextHandlerDictionary(ITashTaskHandlingStatus<IDemoApplicationModel> status) {
            return new Dictionary<string, ISimpleTextHandler> {
                { nameof(status.Model.Alpha), vDemoApplicationHandlers.AlphaTextHandler }
            };
        }

        protected override void OnValueTaskProcessed(ITashTaskHandlingStatus<IDemoApplicationModel> status, bool verify, bool set, string actualValue) {
            if (!verify || actualValue == status.TaskBeingProcessed.Text) {
                status.Model.Status.Type = StatusType.Success;
            } else {
                status.Model.Status.Text = set
                    ? $"Could not set {status.TaskBeingProcessed.ControlName} to \"{status.TaskBeingProcessed.Text}\", it is \"{actualValue}\""
                    : $"Expected {status.TaskBeingProcessed.ControlName} to be \"{status.TaskBeingProcessed.Text}\", got \"{actualValue}\"";
                status.Model.Status.Type = string.IsNullOrEmpty(status.Model.Status.Text) ? StatusType.Success : StatusType.Error;
            }
        }

    }
}
