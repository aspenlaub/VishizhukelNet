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
        public void CanUseContainerBuilder() {
            var logConfigurationMock = new Mock<ILogConfiguration>();
            var container = new ContainerBuilder().UseVishizhukelNetDvinAndPegh(new DummyCsArgumentPrompter(), logConfigurationMock.Object).Build();
            var httpGate = container.Resolve<ISecuredHttpGate>();
            Assert.IsNotNull(httpGate);
        }
    }
}
