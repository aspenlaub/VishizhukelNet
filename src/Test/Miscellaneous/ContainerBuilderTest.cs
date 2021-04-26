using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.Miscellaneous {
    [TestClass]
    public class ContainerBuilderTest {
        [TestMethod]
        public async Task CanUseContainerBuilder() {
            var logConfigurationMock = new Mock<ILogConfiguration>();
            var container = (await new ContainerBuilder().UseVishizhukelNetDvinAndPeghAsync(new DummyCsArgumentPrompter(), logConfigurationMock.Object)).Build();
            var httpGate = container.Resolve<ISecuredHttpGate>();
            Assert.IsNotNull(httpGate);
        }
    }
}
