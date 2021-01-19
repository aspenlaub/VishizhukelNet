using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers {
    public class TashHandlerBase<TModel> : ITashHandler<TModel> where TModel : class, IApplicationModel {
        protected readonly ITashAccessor TashAccessor;
        protected readonly ISimpleLogger SimpleLogger;
        protected readonly ILogConfiguration LogConfiguration;
        protected readonly string LogId;
        protected readonly IButtonNameToCommandMapper ButtonNameToCommandMapper;
        protected readonly IGuiAndAppHandler GuiAndAppHandler;
        protected readonly ITashVerifyAndSetHandler<TModel> TashVerifyAndSetHandler;
        protected readonly ITashSelectorHandler<TModel> TashSelectorHandler;
        protected readonly ITashCommunicator<TModel> TashCommunicator;

        public TashHandlerBase(ITashAccessor tashAccessor, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration,
                IButtonNameToCommandMapper buttonNameToCommandMapper, IGuiAndAppHandler guiAndAppHandler,
                ITashVerifyAndSetHandler<TModel> tashVerifyAndSetHandler, ITashSelectorHandler<TModel> tashSelectorHandler, ITashCommunicator<TModel> tashCommunicator) {
            TashAccessor = tashAccessor;
            SimpleLogger = simpleLogger;
            LogConfiguration = logConfiguration;
            SimpleLogger.LogSubFolder = logConfiguration.LogSubFolder;
            LogId = logConfiguration.LogId;
            ButtonNameToCommandMapper = buttonNameToCommandMapper;
            GuiAndAppHandler = guiAndAppHandler;
            TashVerifyAndSetHandler = tashVerifyAndSetHandler;
            TashSelectorHandler = tashSelectorHandler;
            TashCommunicator = tashCommunicator;
        }

        public async Task<bool> UpdateTashStatusAndReturnIfIsWorkAsync(ITashTaskHandlingStatus<TModel> status) {
            var time = DateTime.Now;
            var statusCode = await TashCommunicator.ConfirmAliveAsync(status, status.Model.IsBusy ? ControllableProcessStatus.Busy : ControllableProcessStatus.Idle, time);
            if (statusCode == HttpStatusCode.NoContent) {
                status.StatusLastConfirmedAt = time;
            }

            await TashCommunicator.ShowStatusAsync(status);
            return await IsThereTashWorkAsync(status);
        }

        protected async Task<bool> IsThereTashWorkAsync(ITashTaskHandlingStatus<TModel> status) {
            if (status.ControllableProcessTasks.Any(t => t.Status == ControllableProcessTaskStatus.Requested || t.Status == ControllableProcessTaskStatus.Processing)) {
                return status.ControllableProcessTasks.Any(t => t.Status == ControllableProcessTaskStatus.Requested);
            }

            var newTask = await TashAccessor.PickRequestedTask(status.ProcessId);
            if (newTask == null) { return false; }

            status.ControllableProcessTasks.Add(newTask);
            return true;
        }

        public async Task ProcessTashAsync(ITashTaskHandlingStatus<TModel> status) {
            using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), LogId))) {
                status.TaskBeingProcessed = status.ControllableProcessTasks.FirstOrDefault(t => t.Status == ControllableProcessTaskStatus.Requested);
                if (status.TaskBeingProcessed == null) {
                    return;
                }

                var statusCode = await TashAccessor.ConfirmStatusAsync(status.TaskBeingProcessed.Id, ControllableProcessTaskStatus.Processing);
                if (statusCode != HttpStatusCode.NoContent) {
                    return;
                }

                await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.Processing);
                OnStatusChangedToProcessingCommunicated(status);

                await TashCommunicator.ShowStatusAsync(status);

                SimpleLogger.LogInformation($"{status.TaskBeingProcessed.Type} requested via remote control");
                await ProcessSingleTaskAsync(status);

                status.TaskBeingProcessed = null;
            }
        }

        protected virtual void OnStatusChangedToProcessingCommunicated(ITashTaskHandlingStatus<TModel> status) {
        }

        protected virtual async Task ProcessSingleTaskAsync(ITashTaskHandlingStatus<TModel> status) {
            using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), LogId))) {
                SimpleLogger.LogInformation($"Processing a task of type {status.TaskBeingProcessed.Type} in {nameof(TashHandlerBase<TModel>)}");

                switch (status.TaskBeingProcessed.Type) {
                    case ControllableProcessTaskType.ProcessTaskList:
                        var taskListTask = status.TaskBeingProcessed;
                        var tasks = JsonConvert.DeserializeObject<List<ControllableProcessTask>>(status.TaskBeingProcessed.Text);
                        SimpleLogger.LogInformation($"Processing a list of {tasks.Count} tasks in {nameof(TashHandlerBase<TModel>)}");
                        foreach (var task in tasks) {
                            status.TaskBeingProcessed = task;
                            SimpleLogger.LogInformation($"Request processing a task of type {task.Type}");
                            await ProcessSingleTaskAsync(status);
                            status.TaskBeingProcessed = taskListTask;
                            if (task.Status == ControllableProcessTaskStatus.Completed) {
                                continue;
                            }

                            SimpleLogger.LogError($"Processing of {tasks.Count} tasks ended incomplete");
                            taskListTask.Status = task.Status;
                            taskListTask.ErrorMessage = task.ErrorMessage;
                            await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, task.Status, false, "", task.ErrorMessage);
                            return;
                        }

                        SimpleLogger.LogInformation($"List of {tasks.Count} tasks processed successfully");
                        await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
                        break;
                    case ControllableProcessTaskType.Maximize:
                        status.Model.WindowState = WindowState.Maximized;
                        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
                        await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
                        break;
                    default:
                        var unknownTaskTypeErrorMessage = $"Unknown task type {status.TaskBeingProcessed.Type}";
                        SimpleLogger.LogError($"Communicating 'BadRequest' to remote controlling process ({unknownTaskTypeErrorMessage}");
                        await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", unknownTaskTypeErrorMessage);
                        break;
                }
            }
        }

        // ReSharper disable once UnusedMember.Global
        protected async Task ProcessPressButtonTaskAsync(ITashTaskHandlingStatus<TModel> status) {
            using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(TashAccessor), LogId))) {
                var command = ButtonNameToCommandMapper.CommandForButton(status.TaskBeingProcessed.ControlName);
                if (command == null) {
                    var errorMessage = $"Unknown control/button {status.TaskBeingProcessed.ControlName}";
                    SimpleLogger.LogError($"Communicating 'BadRequest' to remote controlling process ({errorMessage}");
                    await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                    return;
                }

                await command.ExecuteAsync();
                await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
            }
        }
    }
}
