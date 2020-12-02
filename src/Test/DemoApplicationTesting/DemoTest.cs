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
        private readonly List<int> vAlphaTestValues = new List<int> { 24, 7, 1970, 1 };

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
            await vApplication.Handlers.AlphaTextHandler.TextChangedAsync("24");
            Assert.AreEqual("24", vModel.Alpha.Text);
            Assert.AreEqual(StatusType.None, vModel.Alpha.Type);

            foreach (var text in new[] { "-24", "24abc", "" }) {
                await vApplication.Handlers.AlphaTextHandler.TextChangedAsync(text);
                Assert.AreEqual(text, vModel.Alpha.Text);
                Assert.AreEqual(StatusType.Error, vModel.Alpha.Type);
            }
        }

        [TestMethod]
        public async Task BetaOffersChoicesDependingOnAlpha() {
            foreach (var alpha in vAlphaTestValues) {
                await vApplication.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
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
                await vApplication.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(i);
                Assert.AreEqual(i, vModel.Beta.SelectedIndex);
            }
        }

        [TestMethod]
        public async Task GammaIsEnabledIfAndOnlyIfAlphaIsValidAndBetaHasBeenSelected() {
            foreach (var alphaValid in new[] { false, true }) {
                foreach (var betaSelectionMade in new[] { false, true }) {
                    if (alphaValid) {
                        await vApplication.Handlers.AlphaTextHandler.TextChangedAsync("24");
                    } else {
                        await vApplication.Handlers.AlphaTextHandler.TextChangedAsync("-24");
                    }
                    Assert.AreEqual(alphaValid ? StatusType.None : StatusType.Error, vModel.Alpha.Type);

                    if (betaSelectionMade) {
                        await vApplication.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(1);
                    } else {
                        await vApplication.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(-1);
                    }
                    Assert.AreEqual(betaSelectionMade, vModel.Beta.SelectionMade);

                    var commandEnabled = alphaValid && betaSelectionMade;
                    var hint = "when Alpha is " + (alphaValid ? "valid" : "invalid") + " and Beta selection " + (betaSelectionMade ? "was made" : "was not made");
                    var errorMessage = commandEnabled
                        ? $"Gamma expected to be enabled {hint} but it is not"
                        : $"Gamma expected to be disabled {hint} but it is enabled";
                    Assert.AreEqual(commandEnabled, vModel.Gamma.Enabled, errorMessage);
                }
            }
        }

        [TestMethod]
        public async Task DeltaIsCalculatedAsSumOfAlphaAndBetaWhenGammaWasPressed() {
            foreach (var alpha in vAlphaTestValues) {
                await vApplication.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
                for (var i = 0; i < 4; i++) {
                    await ChangeSelectedBetaIndexAndVerifyResult(i, alpha);
                }
            }
        }

        private async Task ChangeSelectedBetaIndexAndVerifyResult(int i, int alpha) {
            await vApplication.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(i);
            Assert.IsTrue(vModel.Gamma.Enabled);
            await vApplication.Commands.GammaCommand.ExecuteAsync();
            var beta = uint.Parse(vModel.Beta.SelectedItem.Name);
            var expectedResult = vModel.MethodAdd.IsChecked ? alpha + beta : alpha * beta;
            Assert.AreEqual(expectedResult.ToString(), vModel.Delta.Text);
        }

        [TestMethod]
        public async Task DeltaIsClearedWhenAlphaIsChangedAfterRecalculation() {
            var alpha = vAlphaTestValues[0];
            await vApplication.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
            await ChangeSelectedBetaIndexAndVerifyResult(0, alpha);
            await vApplication.Handlers.AlphaTextHandler.TextChangedAsync("4711");
            Assert.IsTrue(vModel.Delta.Text == "");
        }

        [TestMethod]
        public async Task DeltaIsClearedWhenBetaIsChangedAfterRecalculation() {
            var alpha = vAlphaTestValues[0];
            await vApplication.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
            await ChangeSelectedBetaIndexAndVerifyResult(0, alpha);
            await vApplication.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(1);
            Assert.IsTrue(vModel.Delta.Text == "");
        }

        [TestMethod]
        public async Task DeltaIsClearedWhenMethodIsChangedAfterRecalculation() {
            var alpha = vAlphaTestValues[0];
            await vApplication.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
            await ChangeSelectedBetaIndexAndVerifyResult(0, alpha);
            await vApplication.Handlers.MethodMultiplyHandler.ToggledAsync(true);
            Assert.IsTrue(vModel.Delta.Text == "");
            await ChangeSelectedBetaIndexAndVerifyResult(0, alpha);
            await vApplication.Handlers.MethodAddHandler.ToggledAsync(true);
            Assert.IsTrue(vModel.Delta.Text == "");
        }

        [TestMethod]
        public async Task MethodAddIsDeselectedWhenMethodMultiplyIsSelectedAndViceVersa() {
            Assert.IsTrue(vModel.MethodAdd.IsChecked);
            await vApplication.Handlers.MethodMultiplyHandler.ToggledAsync(true);
            Assert.IsTrue(vModel.MethodMultiply.IsChecked);
            Assert.IsFalse(vModel.MethodAdd.IsChecked);
            await vApplication.Handlers.MethodAddHandler.ToggledAsync(true);
            Assert.IsTrue(vModel.MethodAdd.IsChecked);
            Assert.IsFalse(vModel.MethodMultiply.IsChecked);
        }
    }
}
