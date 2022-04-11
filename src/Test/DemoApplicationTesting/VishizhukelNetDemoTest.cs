using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using VishizhukelDemoApplication = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplicationTesting {
    [TestClass]
    public class VishizhukelNetDemoTest {
        private VishizhukelDemoApplication Application;
        private IApplicationModel Model;
        private readonly List<int> AlphaTestValues = new() { 24, 7, 1970, 1 };

        [TestInitialize]
        public async Task Initialize() {
            var logConfigurationMock = new Mock<ILogConfiguration>();
            logConfigurationMock.SetupGet(lc => lc.LogSubFolder).Returns(@"AspenlaubLogs\" + nameof(VishizhukelNetDemoTest));
            logConfigurationMock.SetupGet(lc => lc.LogId).Returns($"{DateTime.Today:yyyy-MM-dd}-{Process.GetCurrentProcess().Id}");
            var container = (await new ContainerBuilder()
                .UseDemoApplicationAsync(null, logConfigurationMock.Object))
                .Build();
            Application = container.Resolve<VishizhukelDemoApplication>();
            Assert.IsNotNull(Application);
            Model = container.Resolve<IApplicationModel>();
            Assert.IsNotNull(Model);
            await Application.OnLoadedAsync();
        }

        [TestMethod]
        public async Task AlphaMustContainPositiveInteger() {
            await Application.Handlers.AlphaTextHandler.TextChangedAsync("24");
            Assert.AreEqual("24", Model.Alpha.Text);
            Assert.AreEqual(StatusType.None, Model.Alpha.Type);

            foreach (var text in new[] { "-24", "24abc", "" }) {
                await Application.Handlers.AlphaTextHandler.TextChangedAsync(text);
                Assert.AreEqual(text, Model.Alpha.Text);
                Assert.AreEqual(StatusType.Error, Model.Alpha.Type);
            }
        }

        [TestMethod]
        public async Task BetaOffersChoicesDependingOnAlpha() {
            foreach (var alpha in AlphaTestValues) {
                await Application.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
                var expectedResult = new List<int> {
                    alpha, alpha + 7, alpha + 24, alpha * 7, alpha * 24
                };
                expectedResult = expectedResult.Distinct().OrderBy(x => x).ToList();
                var actualChoices = Model.Beta.Selectables;
                Assert.AreEqual(expectedResult.Count, actualChoices.Count);
                for (var i = 0; i < expectedResult.Count; i++) {
                    Assert.AreEqual(expectedResult[i].ToString(), actualChoices[i].Name);
                }
            }

            for (var i = 0; i < 4; i++) {
                await Application.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(i);
                Assert.AreEqual(i, Model.Beta.SelectedIndex);
            }
        }

        [TestMethod]
        public async Task GammaIsEnabledIfAndOnlyIfAlphaIsValidAndBetaHasBeenSelected() {
            foreach (var alphaValid in new[] { false, true }) {
                foreach (var betaSelectionMade in new[] { false, true }) {
                    if (alphaValid) {
                        await Application.Handlers.AlphaTextHandler.TextChangedAsync("24");
                    } else {
                        await Application.Handlers.AlphaTextHandler.TextChangedAsync("-24");
                    }
                    Assert.AreEqual(alphaValid ? StatusType.None : StatusType.Error, Model.Alpha.Type);

                    if (betaSelectionMade) {
                        await Application.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(1);
                    } else {
                        await Application.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(-1);
                    }
                    Assert.AreEqual(betaSelectionMade, Model.Beta.SelectionMade);

                    var commandEnabled = alphaValid && betaSelectionMade;
                    var hint = "when Alpha is " + (alphaValid ? "valid" : "invalid") + " and Beta selection " + (betaSelectionMade ? "was made" : "was not made");
                    var errorMessage = commandEnabled
                        ? $"Gamma expected to be enabled {hint} but it is not"
                        : $"Gamma expected to be disabled {hint} but it is enabled";
                    Assert.AreEqual(commandEnabled, Model.Gamma.Enabled, errorMessage);
                }
            }
        }

        [TestMethod]
        public async Task DeltaIsCalculatedAsSumOfAlphaAndBetaWhenGammaWasPressed() {
            foreach (var alpha in AlphaTestValues) {
                await Application.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
                for (var i = 0; i < 4; i++) {
                    await ChangeSelectedBetaIndexAndVerifyResult(i, alpha);
                }
            }
        }

        private async Task ChangeSelectedBetaIndexAndVerifyResult(int i, int alpha) {
            await Application.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(i);
            Assert.IsTrue(Model.Gamma.Enabled);
            await Application.Commands.GammaCommand.ExecuteAsync();
            var beta = uint.Parse(Model.Beta.SelectedItem.Name);
            var expectedResult = Model.MethodAdd.IsChecked ? alpha + beta : alpha * beta;
            Assert.AreEqual(expectedResult.ToString(), Model.Delta.Text);
        }

        [TestMethod]
        public async Task DeltaIsClearedWhenAlphaIsChangedAfterRecalculation() {
            var alpha = AlphaTestValues[0];
            await Application.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
            await ChangeSelectedBetaIndexAndVerifyResult(0, alpha);
            await Application.Handlers.AlphaTextHandler.TextChangedAsync("4711");
            Assert.IsTrue(Model.Delta.Text == "");
        }

        [TestMethod]
        public async Task DeltaIsClearedWhenBetaIsChangedAfterRecalculation() {
            var alpha = AlphaTestValues[0];
            await Application.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
            await ChangeSelectedBetaIndexAndVerifyResult(0, alpha);
            await Application.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(1);
            Assert.IsTrue(Model.Delta.Text == "");
        }

        [TestMethod]
        public async Task DeltaIsClearedWhenMethodIsChangedAfterRecalculation() {
            var alpha = AlphaTestValues[0];
            await Application.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
            await ChangeSelectedBetaIndexAndVerifyResult(0, alpha);
            await Application.Handlers.MethodMultiplyHandler.ToggledAsync(true);
            Assert.IsTrue(Model.Delta.Text == "");
            await ChangeSelectedBetaIndexAndVerifyResult(0, alpha);
            await Application.Handlers.MethodAddHandler.ToggledAsync(true);
            Assert.IsTrue(Model.Delta.Text == "");
        }

        [TestMethod]
        public async Task MethodAddIsDeselectedWhenMethodMultiplyIsSelectedAndViceVersa() {
            Assert.IsTrue(Model.MethodAdd.IsChecked);
            await Application.Handlers.MethodMultiplyHandler.ToggledAsync(true);
            Assert.IsTrue(Model.MethodMultiply.IsChecked);
            Assert.IsFalse(Model.MethodAdd.IsChecked);
            await Application.Handlers.MethodAddHandler.ToggledAsync(true);
            Assert.IsTrue(Model.MethodAdd.IsChecked);
            Assert.IsFalse(Model.MethodMultiply.IsChecked);
        }
    }
}
