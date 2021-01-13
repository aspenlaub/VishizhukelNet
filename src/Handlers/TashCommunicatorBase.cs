using System;
using System.Net;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers {
    public class TashCommunicatorBase<TModel> : ITashCommunicator<TModel> where TModel : IApplicationModel {
        protected readonly ITashAccessor TashAccessor;
        protected readonly ISimpleLogger SimpleLogger;
        protected readonly string LogId;

        public TashCommunicatorBase(ITashAccessor tashAccessor, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration) {
            TashAccessor = tashAccessor;
            SimpleLogger = simpleLogger;
            SimpleLogger.LogSubFolder = logConfiguration.LogSubFolder;
            LogId = logConfiguration.LogId;
        }

        public virtual async Task CommunicateAndShowCompletedOrFailedAsync(ITashTaskHandlingStatus<TModel> status, bool setText, string text) {
            using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), LogId))) {
                if (status.Model.IsModelErroneous(out var errorMessage)) {
                    SimpleLogger.LogInformation($"Communicating 'Failed' with text {errorMessage} to remote controlling process");
                    await ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.Failed, false, "", errorMessage);
                } else {
                    SimpleLogger.LogInformation("Communicating 'Completed' to remote controlling process");
                    await ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.Completed, setText, text, "");
                }
            }
        }

        public async Task ChangeCommunicateAndShowProcessTaskStatusAsync(ITashTaskHandlingStatus<TModel> status,
            ControllableProcessTaskStatus newStatus) {
            await ChangeCommunicateAndShowProcessTaskStatusAsync(status, newStatus, false, "", "");
        }

        public async Task ChangeCommunicateAndShowProcessTaskStatusAsync(ITashTaskHandlingStatus<TModel> status,
                ControllableProcessTaskStatus newStatus, bool setText, string text, string errorMessage) {
            using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), LogId))) {
                status.TaskBeingProcessed.Status = newStatus;
                if (newStatus == ControllableProcessTaskStatus.Failed || newStatus == ControllableProcessTaskStatus.BadRequest) {
                    status.TaskBeingProcessed.ErrorMessage = errorMessage;
                }

                if (setText) {
                    status.TaskBeingProcessed.Text = text;
                }

                SimpleLogger.LogInformation($"Confirm new status of task with id={status.TaskBeingProcessed.Id}");
                await ConfirmStatusOfTaskBeingProcessedAsync(status);
                SimpleLogger.LogInformation($"Confirm that process with id={status.ProcessId} is alive");
                await ConfirmAliveAsync(status, ControllableProcessStatus.Idle, DateTime.Now);
                SimpleLogger.LogInformation($"Show status");
                await ShowStatusAsync(status);
            }
        }

        public async Task ConfirmStatusOfTaskBeingProcessedAsync(ITashTaskHandlingStatus<TModel> status) {
            await TashAccessor.ConfirmStatusAsync(status.TaskBeingProcessed.Id, status.TaskBeingProcessed.Status,
                status.TaskBeingProcessed.Text, status.TaskBeingProcessed.ErrorMessage);
        }

        public async Task<HttpStatusCode> ConfirmAliveAsync(ITashTaskHandlingStatus<TModel> status, ControllableProcessStatus cpStatus, DateTime time) {
            return await TashAccessor.ConfirmAliveAsync(status.ProcessId, time, cpStatus);
        }

        public virtual async Task ShowStatusAsync(ITashTaskHandlingStatus<TModel> status) {
            await Task.Run(() => { });
        }
    }
}
