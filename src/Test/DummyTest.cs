using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test {
    [TestClass]
    public class DummyTest {
        [TestMethod]
        public void NotATestMethod() {
            var container = new ContainerBuilder().UseVishizhukelNetAndPegh(new DummyCsArgumentPrompter()).Build();
            var dummy = container.Resolve<IDummyClass>();
            Assert.IsNotNull(dummy);
        }
    }
}
