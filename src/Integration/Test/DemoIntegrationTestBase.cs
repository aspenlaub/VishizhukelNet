using System.Collections.Generic;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Autofac;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test {
    public class DemoIntegrationTestBase {
        protected readonly IContainer Container;

        public DemoIntegrationTestBase() {
            Container = new ContainerBuilder().RegisterForDemoIntegrationTest().Build();
        }

        protected async Task<DemoWindowUnderTest> CreateDemoWindowUnderTestAsync() {
            var sut = Container.Resolve<DemoWindowUnderTest>();
            await sut.InitializeAsync();
            var process = await sut.FindIdleProcessAsync();
            var tasks = new List<ControllableProcessTask> {
                sut.CreateResetTask(process)
            };
            await sut.RemotelyProcessTaskListAsync(process, tasks);
            return sut;
        }
    }
}
