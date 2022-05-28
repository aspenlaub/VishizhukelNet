using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Web;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.Miscellaneous;

[TestClass]
public class ContainerBuilderTest {
    [TestMethod]
    public async Task CanUseContainerBuilder() {
        var container = (await new ContainerBuilder().UseVishizhukelNetDvinAndPeghAsync("VishizhukelNet", new DummyCsArgumentPrompter())).Build();
        var httpGate = container.Resolve<ISecuredHttpGate>();
        Assert.IsNotNull(httpGate);
    }
}