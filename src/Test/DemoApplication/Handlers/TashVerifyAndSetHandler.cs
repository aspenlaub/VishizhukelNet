using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class TashVerifyAndSetHandler : TashVerifyAndSetHandlerBase<IDemoApplicationModel> {
        private readonly IDemoHandlers vDemoApplicationHandlers;

        public TashVerifyAndSetHandler(IDemoHandlers demoApplicationHandlers, ISimpleLogger simpleLogger, ITashSelectorHandler<IDemoApplicationModel> tashSelectorHandler,
            ITashCommunicator<IDemoApplicationModel> tashCommunicator, Dictionary<string, ISelector> selectors)
            : base(simpleLogger, tashSelectorHandler, tashCommunicator, selectors) {
            vDemoApplicationHandlers = demoApplicationHandlers;
        }

        protected override Dictionary<string, ITextBox> TextBoxNamesToTextBoxDictionary(ITashTaskHandlingStatus<IDemoApplicationModel> status) {
            return new() {
                { nameof(status.Model.Alpha), status.Model.Alpha }
            };
        }

        protected override Dictionary<string, ISimpleTextHandler> TextBoxNamesToTextHandlerDictionary(ITashTaskHandlingStatus<IDemoApplicationModel> status) {
            return new() {
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
