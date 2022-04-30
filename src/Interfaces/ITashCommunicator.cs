using System;
using System.Net;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface ITashCommunicator<in TModel> where TModel : IApplicationModelBase {
    Task CommunicateAndShowCompletedOrFailedAsync(ITashTaskHandlingStatus<TModel> status, bool setText, string text);
    Task ChangeCommunicateAndShowProcessTaskStatusAsync(ITashTaskHandlingStatus<TModel> status, ControllableProcessTaskStatus newStatus);
    Task ChangeCommunicateAndShowProcessTaskStatusAsync(ITashTaskHandlingStatus<TModel> status, ControllableProcessTaskStatus newStatus, bool setText, string text, string errorMessage);
    Task<HttpStatusCode> ConfirmAliveAsync(ITashTaskHandlingStatus<TModel> status, ControllableProcessStatus cpStatus, DateTime time);
    Task ShowStatusAsync(ITashTaskHandlingStatus<TModel> status);
}