using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Components;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VishizhukelDemoApplication = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application.DemoApplication;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplicationTesting {
    [TestClass]
    public class DemoTest {
        private VishizhukelDemoApplication vApplication;
        private IDemoApplicationModel vModel;

        [TestInitialize]
        public void Initialize() {
            var container = new ContainerBuilder()
                .UseVishizhukelNetAndPegh(new DummyCsArgumentPrompter())
                .UseDemoApplication(true)
                .Build();
            vApplication = container.Resolve<VishizhukelDemoApplication>();
            Assert.IsNotNull(vApplication);
            vModel = container.Resolve<IDemoApplicationModel>();
            Assert.IsNotNull(vModel);
        }

        [TestMethod]
        public async Task AlphaMustContainPositiveInteger() {
            await vApplication.AlphaTextChangedAsync("24");
            Assert.AreEqual("24", vModel.Alpha.Text);
            Assert.AreEqual(StatusType.Success, vModel.Alpha.Type);

            foreach(var text in new[] { "-24", "24abc", "" }) {
                await vApplication.AlphaTextChangedAsync(text);
                Assert.AreEqual(text, vModel.Alpha.Text);
                Assert.AreEqual(StatusType.Error, vModel.Alpha.Type);
            }
        }
    }
}
