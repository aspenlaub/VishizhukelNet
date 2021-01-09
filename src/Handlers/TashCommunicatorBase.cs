using System;
using System.Net;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers {
    public class TashCommunicatorBase<TModel> : ITashCommunicator<TModel> where TModel : IApplicationModel {
        private readonly ITashAccessor vTashAccessor;
        private readonly IApplicationLogger vApplicationLogger;

        public TashCommunicatorBase(ITashAccessor tashAccessor, IApplicationLogger applicationLogger) {
            vTashAccessor = tashAccessor;
            vApplicationLogger = applicationLogger;
        }

        public async Task CommunicateAndShowCompletedOrFailedAsync(ITashTaskHandlingStatus<TModel> status, bool setText, string text) {
            vApplicationLogger.LogMessage("Communicating 'Completed' to remote controlling process");
            await ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.Completed, setText, text, "");
        }

        public async Task ChangeCommunicateAndShowProcessTaskStatusAsync(ITashTaskHandlingStatus<TModel> status,
            ControllableProcessTaskStatus newStatus) {
            await ChangeCommunicateAndShowProcessTaskStatusAsync(status, newStatus, false, "", "");
        }

        public async Task ChangeCommunicateAndShowProcessTaskStatusAsync(ITashTaskHandlingStatus<TModel> status,
            ControllableProcessTaskStatus newStatus, bool setText, string text, string errorMessage) {
            status.TaskBeingProcessed.Status = newStatus;
            if (newStatus == ControllableProcessTaskStatus.Failed || newStatus == ControllableProcessTaskStatus.BadRequest) {
                status.TaskBeingProcessed.ErrorMessage = errorMessage;
            }

            if (setText) {
                status.TaskBeingProcessed.Text = text;
            }

            await vTashAccessor.ConfirmStatusAsync(status.TaskBeingProcessed.Id, status.TaskBeingProcessed.Status, status.TaskBeingProcessed.Text, status.TaskBeingProcessed.ErrorMessage);
            await ConfirmAliveAsync(status, ControllableProcessStatus.Idle, DateTime.Now);
            await ShowStatusAsync(status);
        }

        public async Task<HttpStatusCode> ConfirmAliveAsync(ITashTaskHandlingStatus<TModel> status, ControllableProcessStatus cpStatus, DateTime time) {
            return await vTashAccessor.ConfirmAliveAsync(status.ProcessId, time, cpStatus);
        }

        public async Task ShowStatusAsync(ITashTaskHandlingStatus<TModel> status) {
            await Task.Run(() => { });
        }
    }
}
