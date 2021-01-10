﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Helpers;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ControllableProcessTaskType = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities.ControllableProcessTaskType;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    public class DemoWindowUnderTestActions {
        private readonly ITashAccessor vTashAccessor;
        private bool vInitialized;

        public DemoWindowUnderTestActions(ITashAccessor tashAccessor) {
            vTashAccessor = tashAccessor;
            vInitialized = false;
        }

        public virtual async Task InitializeAsync() {
            var errorsAndInfos = await vTashAccessor.EnsureTashAppIsRunningAsync();
            Assert.IsFalse(errorsAndInfos.AnyErrors(), string.Join("\r\n", errorsAndInfos.Errors));

            await vTashAccessor.AssumeDeath(p => p.Title == "Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test");

            var processes = await vTashAccessor.GetControllableProcessesAsync();
            Assert.IsFalse(processes.Any(p => p.Title == "Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test" && p.Status != ControllableProcessStatus.Dead), "Non-dead processes exist before test execution");
            vInitialized = true;
        }

        public async Task<ControllableProcess> FindIdleProcessAsync() {
            if (!vInitialized) { throw new Exception("InitializeAsync has not been called"); }

            ControllableProcess process = null;
            await Wait.UntilAsync(async () => (process = await TryFindIdleProcess()) != null, TimeSpan.FromSeconds(30));
            Assert.IsNotNull(process);
            return process;
        }

        protected async Task<ControllableProcess> TryFindIdleProcess() {
            var findIdleProcessResult = await vTashAccessor.FindIdleProcess(p => p.Title == "Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test");
            return findIdleProcessResult.ControllableProcess;
        }

        public ControllableProcessTask CreateResetTask(ControllableProcess process) {
            return CreateControllableProcessTask(process, ControllableProcessTaskType.Reset, "", "");
        }

        public async Task RemotelyProcessTaskListAsync(ControllableProcess process, List<ControllableProcessTask> tasks) {
            var task = CreateControllableProcessTask(process, ControllableProcessTaskType.ProcessTaskList, "", JsonConvert.SerializeObject(tasks));
            await SubmitNewTaskAndAwaitCompletionAsync(task);
        }

        public ControllableProcessTask CreateControllableProcessTask(ControllableProcess process, string type, string controlName, string text) {
            return new ControllableProcessTask {
                Id = Guid.NewGuid(),
                ProcessId = process.ProcessId,
                Type = type,
                ControlName = controlName,
                Status = ControllableProcessTaskStatus.Requested,
                Text = text
            };
        }

        public async Task<string> SubmitNewTaskAndAwaitCompletionAsync(ControllableProcessTask task) {
            return await SubmitNewTaskAndAwaitCompletionAsync(task, true);
        }

        public async Task<string> SubmitNewTaskAndAwaitCompletionAsync(ControllableProcessTask task, bool successIsExpected) {
            var status = await vTashAccessor.PutControllableProcessTaskAsync(task);
            Assert.AreEqual(HttpStatusCode.Created, status);

            await vTashAccessor.AwaitCompletionAsync(task.Id, 20000);
            var result = vTashAccessor.GetControllableProcessTaskAsync(task.Id).Result;
            if (successIsExpected) {
                var errorMessage = $"Task status is {Enum.GetName(typeof(ControllableProcessTaskStatus), result.Status)}, error message: {result.ErrorMessage}";
                Assert.AreEqual(ControllableProcessTaskStatus.Completed, result.Status, errorMessage);
                return result.Text;
            } else {
                var errorMessage = $"Unexpected task status {Enum.GetName(typeof(ControllableProcessTaskStatus), result.Status)}";
                Assert.AreEqual(ControllableProcessTaskStatus.Failed, result.Status, errorMessage);
                return result.ErrorMessage;
            }
        }
    }
}
