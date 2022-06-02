using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VishizhukelDemoApplication = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplicationTesting;

[TestClass]
public class VishizhukelNetDemoTest {
    private VishizhukelDemoApplication _Application;
    private IApplicationModel _Model;
    private readonly List<int> _AlphaTestValues = new() { 24, 7, 1970, 1 };

    [TestInitialize]
    public async Task Initialize() {
        var container = (await new ContainerBuilder().UseDemoApplicationAsync(null)) .Build();
        _Application = container.Resolve<VishizhukelDemoApplication>();
        Assert.IsNotNull(_Application);
        _Model = container.Resolve<IApplicationModel>();
        Assert.IsNotNull(_Model);
        await _Application.OnLoadedAsync();
    }

    [TestMethod]
    public async Task AlphaMustContainPositiveInteger() {
        await _Application.Handlers.AlphaTextHandler.TextChangedAsync("24");
        Assert.AreEqual("24", _Model.Alpha.Text);
        Assert.AreEqual(StatusType.None, _Model.Alpha.Type);

        foreach (var text in new[] { "-24", "24abc", "" }) {
            await _Application.Handlers.AlphaTextHandler.TextChangedAsync(text);
            Assert.AreEqual(text, _Model.Alpha.Text);
            Assert.AreEqual(StatusType.Error, _Model.Alpha.Type);
        }
    }

    [TestMethod]
    public async Task BetaOffersChoicesDependingOnAlpha() {
        foreach (var alpha in _AlphaTestValues) {
            await _Application.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
            var expectedResult = new List<int> {
                alpha, alpha + 7, alpha + 24, alpha * 7, alpha * 24
            };
            expectedResult = expectedResult.Distinct().OrderBy(x => x).ToList();
            var actualChoices = _Model.Beta.Selectables;
            Assert.AreEqual(expectedResult.Count, actualChoices.Count);
            for (var i = 0; i < expectedResult.Count; i++) {
                Assert.AreEqual(expectedResult[i].ToString(), actualChoices[i].Name);
            }
        }

        for (var i = 0; i < 4; i++) {
            await _Application.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(i);
            Assert.AreEqual(i, _Model.Beta.SelectedIndex);
        }
    }

    [TestMethod]
    public async Task GammaIsEnabledIfAndOnlyIfAlphaIsValidAndBetaHasBeenSelected() {
        foreach (var alphaValid in new[] { false, true }) {
            foreach (var betaSelectionMade in new[] { false, true }) {
                if (alphaValid) {
                    await _Application.Handlers.AlphaTextHandler.TextChangedAsync("24");
                } else {
                    await _Application.Handlers.AlphaTextHandler.TextChangedAsync("-24");
                }
                Assert.AreEqual(alphaValid ? StatusType.None : StatusType.Error, _Model.Alpha.Type);

                if (betaSelectionMade) {
                    await _Application.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(1);
                } else {
                    await _Application.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(-1);
                }
                Assert.AreEqual(betaSelectionMade, _Model.Beta.SelectionMade);

                var commandEnabled = alphaValid && betaSelectionMade;
                var hint = "when Alpha is " + (alphaValid ? "valid" : "invalid") + " and Beta selection " + (betaSelectionMade ? "was made" : "was not made");
                var errorMessage = commandEnabled
                    ? $"Gamma expected to be enabled {hint} but it is not"
                    : $"Gamma expected to be disabled {hint} but it is enabled";
                Assert.AreEqual(commandEnabled, _Model.Gamma.Enabled, errorMessage);
            }
        }
    }

    [TestMethod]
    public async Task DeltaIsCalculatedAsSumOfAlphaAndBetaWhenGammaWasPressed() {
        foreach (var alpha in _AlphaTestValues) {
            await _Application.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
            for (var i = 0; i < 4; i++) {
                await ChangeSelectedBetaIndexAndVerifyResult(i, alpha);
            }
        }
    }

    private async Task ChangeSelectedBetaIndexAndVerifyResult(int i, int alpha) {
        await _Application.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(i);
        Assert.IsTrue(_Model.Gamma.Enabled);
        await _Application.Commands.GammaCommand.ExecuteAsync();
        var beta = uint.Parse(_Model.Beta.SelectedItem.Name);
        var expectedResult = _Model.MethodAdd.IsChecked ? alpha + beta : alpha * beta;
        Assert.AreEqual(expectedResult.ToString(), _Model.Delta.Text);
    }

    [TestMethod]
    public async Task DeltaIsClearedWhenAlphaIsChangedAfterRecalculation() {
        var alpha = _AlphaTestValues[0];
        await _Application.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
        await ChangeSelectedBetaIndexAndVerifyResult(0, alpha);
        await _Application.Handlers.AlphaTextHandler.TextChangedAsync("4711");
        Assert.IsTrue(_Model.Delta.Text == "");
    }

    [TestMethod]
    public async Task DeltaIsClearedWhenBetaIsChangedAfterRecalculation() {
        var alpha = _AlphaTestValues[0];
        await _Application.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
        await ChangeSelectedBetaIndexAndVerifyResult(0, alpha);
        await _Application.Handlers.BetaSelectorHandler.SelectedIndexChangedAsync(1);
        Assert.IsTrue(_Model.Delta.Text == "");
    }

    [TestMethod]
    public async Task DeltaIsClearedWhenMethodIsChangedAfterRecalculation() {
        var alpha = _AlphaTestValues[0];
        await _Application.Handlers.AlphaTextHandler.TextChangedAsync(alpha.ToString());
        await ChangeSelectedBetaIndexAndVerifyResult(0, alpha);
        await _Application.Handlers.MethodMultiplyHandler.ToggledAsync(true);
        Assert.IsTrue(_Model.Delta.Text == "");
        await ChangeSelectedBetaIndexAndVerifyResult(0, alpha);
        await _Application.Handlers.MethodAddHandler.ToggledAsync(true);
        Assert.IsTrue(_Model.Delta.Text == "");
    }

    [TestMethod]
    public async Task MethodAddIsDeselectedWhenMethodMultiplyIsSelectedAndViceVersa() {
        Assert.IsTrue(_Model.MethodAdd.IsChecked);
        await _Application.Handlers.MethodMultiplyHandler.ToggledAsync(true);
        Assert.IsTrue(_Model.MethodMultiply.IsChecked);
        Assert.IsFalse(_Model.MethodAdd.IsChecked);
        await _Application.Handlers.MethodAddHandler.ToggledAsync(true);
        Assert.IsTrue(_Model.MethodAdd.IsChecked);
        Assert.IsFalse(_Model.MethodMultiply.IsChecked);
    }
}