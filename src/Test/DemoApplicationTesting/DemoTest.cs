using System.Collections.Generic;
using System.Linq;
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
                .UseDemoApplication(null)
                .Build();
            vApplication = container.Resolve<VishizhukelDemoApplication>();
            Assert.IsNotNull(vApplication);
            vModel = container.Resolve<IDemoApplicationModel>();
            Assert.IsNotNull(vModel);
            vApplication.RegisterTypes();
        }

        [TestMethod]
        public async Task AlphaMustContainPositiveInteger() {
            await vApplication.AlphaTextChangedAsync("24");
            Assert.AreEqual("24", vModel.Alpha.Text);
            Assert.AreEqual(StatusType.Success, vModel.Alpha.Type);

            foreach (var text in new[] { "-24", "24abc", "" }) {
                await vApplication.AlphaTextChangedAsync(text);
                Assert.AreEqual(text, vModel.Alpha.Text);
                Assert.AreEqual(StatusType.Error, vModel.Alpha.Type);
            }
        }

        [TestMethod]
        public async Task BetaOffersChoicesDependingOnAlpha() {
            foreach(var alpha in new[] { 24, 7, 1970, 1 }) {
                // Alpha, Alpha + 7, Alpha + 24, Alpha * 7 and Alpha * 24
                await vApplication.AlphaTextChangedAsync(alpha.ToString());
                var expectedResult = new List<int> {
                    alpha, alpha + 7, alpha + 24, alpha * 7, alpha * 24
                };
                expectedResult = expectedResult.Distinct().OrderBy(x => x).ToList();
                var actualChoices = vModel.Beta.Selectables;
                Assert.AreEqual(expectedResult.Count, actualChoices.Count);
                for (var i = 0; i < expectedResult.Count; i++) {
                    Assert.AreEqual(expectedResult[i].ToString(), actualChoices[i].Name);
                }
            }

            for (var i = 0; i < 4; i++) {
                await vApplication.Handlers.BetaSelectorHandler.SelectedBetaIndexChangedAsync(i);
                Assert.AreEqual(i, vModel.Beta.SelectedIndex);
            }
        }
    }
}
