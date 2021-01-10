using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using ControllableProcessTaskType = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities.ControllableProcessTaskType;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class TashHandler : TashHandlerBase<IDemoApplicationModel> {
        public TashHandler(ITashAccessor tashAccessor, IApplicationLogger applicationLogger,
            IButtonNameToCommandMapper buttonNameToCommandMapper,
            ITashVerifyAndSetHandler<IDemoApplicationModel> tashVerifyAndSetHandler, ITashSelectorHandler<IDemoApplicationModel> tashSelectorHandler, ITashCommunicator<IDemoApplicationModel> tashCommunicator)
            : base(tashAccessor, applicationLogger, buttonNameToCommandMapper, tashVerifyAndSetHandler, tashSelectorHandler, tashCommunicator) {
        }

        protected override async Task ProcessSingleTaskAsync(ITashTaskHandlingStatus<IDemoApplicationModel> status) {
            ApplicationLogger.LogMessage($"Processing a task of type {status.TaskBeingProcessed.Type} in {nameof(TashHandler)}");

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
}
