using System;
using System.Net;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers {
    public class TashCommunicatorBase<TModel> : ITashCommunicator<TModel> where TModel : IApplicationModel {
        protected readonly ITashAccessor TashAccessor;
        protected readonly IApplicationLogger ApplicationLogger;

        public TashCommunicatorBase(ITashAccessor tashAccessor, IApplicationLogger applicationLogger) {
            TashAccessor = tashAccessor;
            ApplicationLogger = applicationLogger;
        }

        public virtual async Task CommunicateAndShowCompletedOrFailedAsync(ITashTaskHandlingStatus<TModel> status, bool setText, string text) {
            if (status.Model.IsModelErroneous(out var errorMessage)) {
                ApplicationLogger.LogMessage($"Communicating 'Failed' with text {errorMessage} to remote controlling process");
                await ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.Failed, false, "", errorMessage);
            } else {
                ApplicationLogger.LogMessage("Communicating 'Completed' to remote controlling process");
                await ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.Completed, setText, text, "");
            }
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

            // TODO: reduce to one attempt
            var attempts = 10;
            HttpStatusCode statusCode;
            do {
                statusCode = await TashAccessor.ConfirmStatusAsync(status.TaskBeingProcessed.Id, status.TaskBeingProcessed.Status,
                    status.TaskBeingProcessed.Text, status.TaskBeingProcessed.ErrorMessage);
            } while (statusCode == HttpStatusCode.NotFound && --attempts >= 0);

            await ConfirmAliveAsync(status, ControllableProcessStatus.Idle, DateTime.Now);
            await ShowStatusAsync(status);
        }

        public async Task<HttpStatusCode> ConfirmAliveAsync(ITashTaskHandlingStatus<TModel> status, ControllableProcessStatus cpStatus, DateTime time) {
            return await TashAccessor.ConfirmAliveAsync(status.ProcessId, time, cpStatus);
        }

        public virtual async Task ShowStatusAsync(ITashTaskHandlingStatus<TModel> status) {
            await Task.Run(() => { });
        }
    }
}
