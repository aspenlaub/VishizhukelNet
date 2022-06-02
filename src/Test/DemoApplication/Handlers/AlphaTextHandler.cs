using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;

public class AlphaTextHandler : ISimpleTextHandler {
    private readonly IApplicationModel _Model;
    private readonly IGuiAndAppHandler<ApplicationModel> _GuiAndAppHandler;
    private readonly ISimpleSelectorHandler _BetaSelectorHandler;
    private readonly ISimpleTextHandler _DeltaTextHandler;

    public AlphaTextHandler(IApplicationModel model, IGuiAndAppHandler<ApplicationModel> guiAndAppHandler, ISimpleSelectorHandler betaSelectorHandler, ISimpleTextHandler deltaTextHandler) {
        _Model = model;
        _GuiAndAppHandler = guiAndAppHandler;
        _BetaSelectorHandler = betaSelectorHandler;
        _DeltaTextHandler = deltaTextHandler;
    }

    public async Task TextChangedAsync(string text) {
        if (_Model.Alpha.Text == text) { return; }

        _Model.Alpha.Text = text;
        _Model.Alpha.Type = uint.TryParse(text, out _) ? StatusType.None : StatusType.Error;
        await _BetaSelectorHandler.UpdateSelectableValuesAsync();
        await _DeltaTextHandler.TextChangedAsync("");
        await _GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }
}