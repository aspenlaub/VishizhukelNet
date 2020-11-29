using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DummyApplication {
    [TestClass]
    public class DummyTest {
        [TestMethod]
        public void NotATestMethod() {
            var container = new ContainerBuilder()
                .UseVishizhukelNetAndPegh(new DummyCsArgumentPrompter())
                .UseDummyApplication()
                .Build();
            var application = container.Resolve<DummyApplication>();
            Assert.IsNotNull(application);
            var guiAndApplicationSynchronizer = container.Resolve<DummyGuiAndApplicationSynchronizer>();
            Assert.IsNotNull(guiAndApplicationSynchronizer);
        }
    }
}
