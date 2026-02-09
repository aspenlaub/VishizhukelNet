using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.Miscellaneous;

[TestClass]
public class ContainerBuilderTest {
    [TestMethod]
    public async Task CanUseContainerBuilder() {
        IContainer container = (await new ContainerBuilder().UseVishizhukelNetDvinAndPeghAsync("VishizhukelNet")).Build();
        ISecuredHttpGate httpGate = container.Resolve<ISecuredHttpGate>();
        Assert.IsNotNull(httpGate);
    }
}