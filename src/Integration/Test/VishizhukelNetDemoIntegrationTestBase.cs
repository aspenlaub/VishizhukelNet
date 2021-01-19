using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Autofac;
using Moq;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    public class VishizhukelNetDemoIntegrationTestBase {
        protected readonly IContainer Container;

        public VishizhukelNetDemoIntegrationTestBase() {
            var logConfigurationMock = new Mock<ILogConfiguration>();
            logConfigurationMock.SetupGet(lc => lc.LogSubFolder).Returns(@"AspenlaubLogs\" + nameof(VishizhukelNetDemoIntegrationTestBase));
            logConfigurationMock.SetupGet(lc => lc.LogId).Returns($"{DateTime.Today:yyyy-MM-dd}-{Process.GetCurrentProcess().Id}");
            Container = new ContainerBuilder().RegisterForDemoIntegrationTest(logConfigurationMock.Object).Build();
        }

        protected async Task<DemoWindowUnderTest> CreateDemoWindowUnderTestAsync() {
            var sut = Container.Resolve<DemoWindowUnderTest>();
            await sut.InitializeAsync();
            var process = await sut.FindIdleProcessAsync();
            var tasks = new List<ControllableProcessTask> {
                sut.CreateResetTask(process),
                sut.CreateMaximizeTask(process)
            };
            await sut.RemotelyProcessTaskListAsync(process, tasks);
            return sut;
        }
    }
}
