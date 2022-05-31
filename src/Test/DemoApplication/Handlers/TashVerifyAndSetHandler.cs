using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;

public class TashVerifyAndSetHandler : TashVerifyAndSetHandlerBase<IApplicationModel> {
    private readonly IApplicationHandlers DemoApplicationHandlers;

    public TashVerifyAndSetHandler(IApplicationHandlers demoApplicationHandlers, ISimpleLogger simpleLogger, ITashSelectorHandler<IApplicationModel> tashSelectorHandler,
        ITashCommunicator<IApplicationModel> tashCommunicator, Dictionary<string, ISelector> selectors, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor)
        : base(simpleLogger, tashSelectorHandler, tashCommunicator, selectors, methodNamesFromStackFramesExtractor) {
        DemoApplicationHandlers = demoApplicationHandlers;
    }

    protected override Dictionary<string, ITextBox> TextBoxNamesToTextBoxDictionary(ITashTaskHandlingStatus<IApplicationModel> status) {
        return new() {
            { nameof(status.Model.Alpha), status.Model.Alpha }
        };
    }

    protected override Dictionary<string, ISimpleTextHandler> TextBoxNamesToTextHandlerDictionary(ITashTaskHandlingStatus<IApplicationModel> status) {
        return new() {
            { nameof(status.Model.Alpha), DemoApplicationHandlers.AlphaTextHandler }
        };
    }

    protected override Dictionary<string, ICollectionViewSource> CollectionViewSourceNamesToCollectionViewSourceDictionary(ITashTaskHandlingStatus<IApplicationModel> status) {
        return new() {
            { nameof(status.Model.Theta), status.Model.Theta }
        };
    }

    protected override Dictionary<string, ISimpleCollectionViewSourceHandler> CollectionViewSourceNamesToCollectionViewSourceHandlerDictionary(ITashTaskHandlingStatus<IApplicationModel> status) {
        return new() {
            { nameof(status.Model.Theta), DemoApplicationHandlers.ThetaHandler }
        };
    }

    protected override void OnValueTaskProcessed(ITashTaskHandlingStatus<IApplicationModel> status, bool verify, bool set, string actualValue) {
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