using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplicationTesting {
    [TestClass]
    public class DemoTest {
        [TestMethod]
        public void CanResolveDemoApplication() {
            var container = new ContainerBuilder()
                .UseVishizhukelNetAndPegh(new DummyCsArgumentPrompter())
                .UseDummyApplication(true)
                .Build();
            var application = container.Resolve<DemoApplication.Application.DemoApplication>();
            Assert.IsNotNull(application);
        }
    }
}
