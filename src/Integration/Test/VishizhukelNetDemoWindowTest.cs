using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    [TestClass]
    public class VishizhukelNetDemoWindowTest : VishizhukelNetDemoIntegrationTestBase {
        [TestMethod]
        public async Task CanOpenAndMaximizeDemoWindow() {
            using var sut = await CreateDemoWindowUnderTestAsync();
            var process = await sut.FindIdleProcessAsync();
            var tasks = new List<ControllableProcessTask> {
                sut.CreateMaximizeTask(process)
            };
            await sut.RemotelyProcessTaskListAsync(process, tasks);
        }

        [TestMethod]
        public async Task CanCalculateSum() {
            using var sut = await CreateDemoWindowUnderTestAsync();
            var process = await sut.FindIdleProcessAsync();
            var tasks = new List<ControllableProcessTask> {
                sut.CreateVerifyWhetherEnabledTask(process, nameof(IDemoApplicationModel.Beta), false),
                sut.CreateSetValueTask(process, nameof(IDemoApplicationModel.Alpha), "7"),
                sut.CreateVerifyWhetherEnabledTask(process, nameof(IDemoApplicationModel.Beta), true),
                sut.CreateVerifyNumberOfItemsTask(process, nameof(IDemoApplicationModel.Beta), 5),
                sut.CreateSelectBetaTask(process, "49")
            };
            await sut.RemotelyProcessTaskListAsync(process, tasks);
        }
    }
}
