using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Enums;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Entities;
using IApplicationModel = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Interfaces.IApplicationModel;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Handlers;

public class TashHandler : TashHandlerBase<ApplicationModel> {
    public TashHandler(ITashAccessor tashAccessor, ISimpleLogger simpleLogger, IButtonNameToCommandMapper buttonNameToCommandMapper,
            IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper, IGuiAndAppHandler<ApplicationModel> guiAndAppHandler,
            ITashVerifyAndSetHandler<IApplicationModel> tashVerifyAndSetHandler, ITashSelectorHandler<IApplicationModel> tashSelectorHandler,
            ITashCommunicator<IApplicationModel> tashCommunicator, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor)
        : base(tashAccessor, simpleLogger, buttonNameToCommandMapper, toggleButtonNameToHandlerMapper, guiAndAppHandler, tashVerifyAndSetHandler, tashSelectorHandler, tashCommunicator, methodNamesFromStackFramesExtractor) {
    }

    protected override async Task ProcessSingleTaskAsync(ITashTaskHandlingStatus<ApplicationModel> status) {
        var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
        SimpleLogger.LogInformationWithCallStack($"Processing a task of type {status.TaskBeingProcessed.Type} in {nameof(TashHandler)}", methodNamesFromStack);

        switch (status.TaskBeingProcessed.Type) {
            case ControllableProcessTaskType.Reset:
                await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
                break;
            default:
                await base.ProcessSingleTaskAsync(status);
                break;
        }
    }
}