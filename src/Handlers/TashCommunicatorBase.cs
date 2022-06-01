using System;
using System.Net;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Extensions;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;

public class TashCommunicatorBase<TModel> : ITashCommunicator<TModel> where TModel : IApplicationModelBase {
    protected readonly ITashAccessor TashAccessor;
    protected readonly ISimpleLogger SimpleLogger;
    protected readonly IMethodNamesFromStackFramesExtractor MethodNamesFromStackFramesExtractor;

    public TashCommunicatorBase(ITashAccessor tashAccessor, ISimpleLogger simpleLogger, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor) {
        TashAccessor = tashAccessor ?? throw new ArgumentNullException(nameof(tashAccessor));
        SimpleLogger = simpleLogger ?? throw new ArgumentNullException(nameof(simpleLogger));
        MethodNamesFromStackFramesExtractor = methodNamesFromStackFramesExtractor ?? throw new ArgumentNullException(nameof(methodNamesFromStackFramesExtractor));
    }

    public virtual async Task CommunicateAndShowCompletedOrFailedAsync(ITashTaskHandlingStatus<TModel> status, bool setText, string text) {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.CreateWithRandomId(nameof(CommunicateAndShowCompletedOrFailedAsync)))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            if (status.Model.Status.Type == StatusType.Error) {
                var errorMessage = status.Model.Status.Text;
                SimpleLogger.LogInformationWithCallStack($"Communicating 'Failed' with text {errorMessage} to remote controlling process", methodNamesFromStack);
                await ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.Failed, false, "", errorMessage);
            } else {
                SimpleLogger.LogInformationWithCallStack("Communicating 'Completed' to remote controlling process", methodNamesFromStack);
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
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.CreateWithRandomId(nameof(ChangeCommunicateAndShowProcessTaskStatusAsync)))) {
            var methodNamesFromStack = MethodNamesFromStackFramesExtractor.ExtractMethodNamesFromStackFrames();
            status.TaskBeingProcessed.Status = newStatus;
            if (newStatus == ControllableProcessTaskStatus.Failed || newStatus == ControllableProcessTaskStatus.BadRequest) {
                status.TaskBeingProcessed.ErrorMessage = errorMessage;
            }

            if (setText) {
                status.TaskBeingProcessed.Text = text;
            }

            SimpleLogger.LogInformationWithCallStack($"Confirm new status of task with id={status.TaskBeingProcessed.Id}", methodNamesFromStack);
            await ConfirmStatusOfTaskBeingProcessedAsync(status);
            SimpleLogger.LogInformationWithCallStack($"Confirm that process with id={status.ProcessId} is alive", methodNamesFromStack);
            await ConfirmAliveAsync(status, ControllableProcessStatus.Idle, DateTime.Now);
            SimpleLogger.LogInformationWithCallStack("Show status", methodNamesFromStack);
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