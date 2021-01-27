using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishnetIntegrationTestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ControllableProcessTaskType = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities.ControllableProcessTaskType;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    public class DemoWindowUnderTestActions : WindowUnderTestActionsBase {
        public DemoWindowUnderTestActions(ITashAccessor tashAccessor) : base(tashAccessor, "Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test") {
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
            var status = await TashAccessor.PutControllableProcessTaskAsync(task);
            Assert.AreEqual(HttpStatusCode.Created, status);

            await TashAccessor.AwaitCompletionAsync(task.Id, 20000);
            var result = TashAccessor.GetControllableProcessTaskAsync(task.Id).Result;
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

        public ControllableProcessTask CreateMaximizeTask(ControllableProcess process) {
            return CreateControllableProcessTask(process, ControllableProcessTaskType.Maximize, "", "");
        }

        public ControllableProcessTask CreateSetValueTask(ControllableProcess process, string controlName, string value) {
            return CreateControllableProcessTask(process, ControllableProcessTaskType.SetValue, controlName, value);
        }

        public ControllableProcessTask CreateSelectBetaTask(ControllableProcess process, string beta) {
            return CreateControllableProcessTask(process, ControllableProcessTaskType.SelectComboItem, nameof(IDemoApplicationModel.Beta), beta);
        }

        public ControllableProcessTask CreateVerifyWhetherEnabledTask(ControllableProcess process, string controlName, bool enabled) {
            return CreateControllableProcessTask(process, ControllableProcessTaskType.VerifyWhetherEnabled, controlName, enabled ? "true" : "false");
        }

        public ControllableProcessTask CreateVerifyNumberOfItemsTask(ControllableProcess process, string controlName, int expectedNumberOfItems) {
            return CreateControllableProcessTask(process, ControllableProcessTaskType.VerifyNumberOfItems, controlName, expectedNumberOfItems.ToString());
        }
    }
}
