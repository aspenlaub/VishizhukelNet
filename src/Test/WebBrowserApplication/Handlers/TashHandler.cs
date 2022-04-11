﻿using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Enums;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;
using IApplicationModel = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces.IApplicationModel;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Handlers {
    public class TashHandler : TashHandlerBase<IApplicationModel> {
        public TashHandler(ITashAccessor tashAccessor, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration,
            IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper, IGuiAndAppHandler guiAndAppHandler,
            ITashVerifyAndSetHandler<IApplicationModel> tashVerifyAndSetHandler, ITashSelectorHandler<IApplicationModel> tashSelectorHandler, ITashCommunicator<IApplicationModel> tashCommunicator)
            : base(tashAccessor, simpleLogger, logConfiguration, buttonNameToCommandMapper, toggleButtonNameToHandlerMapper, guiAndAppHandler, tashVerifyAndSetHandler, tashSelectorHandler, tashCommunicator) {
        }

        protected override async Task ProcessSingleTaskAsync(ITashTaskHandlingStatus<IApplicationModel> status) {
            SimpleLogger.LogInformation($"Processing a task of type {status.TaskBeingProcessed.Type} in {nameof(TashHandler)}");

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