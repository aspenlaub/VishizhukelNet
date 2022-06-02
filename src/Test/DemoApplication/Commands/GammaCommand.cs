using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Commands;

public class GammaCommand : ICommand {
    private readonly IApplicationModel _Model;
    private readonly ISimpleTextHandler _DeltaTextHandler;

    public GammaCommand(IApplicationModel model, ISimpleTextHandler deltaTextHandler) {
        _Model = model;
        _DeltaTextHandler = deltaTextHandler;
    }

    public async Task ExecuteAsync() {
        if (!_Model.Gamma.Enabled) {
            return;
        }

        var alphaValue = uint.Parse(_Model.Alpha.Text);
        var betaValue = uint.Parse(_Model.Beta.SelectedItem.Name);
        var result = _Model.MethodAdd.IsChecked ? alphaValue + betaValue : alphaValue * betaValue;
        await _DeltaTextHandler.TextChangedAsync(result.ToString());
        _Model.Status.Type = StatusType.Success;
        _Model.Status.Text = Properties.Resources.CalculationSuccessful;
    }

    public async Task<bool> ShouldBeEnabledAsync() {
        var enabled = _Model.Alpha.Type == StatusType.None && _Model.Beta.SelectionMade;
        return await Task.FromResult(enabled);
    }
}