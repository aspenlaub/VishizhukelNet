using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;

public class DeltaTextHandler : ISimpleTextHandler {
    private readonly IApplicationModel _Model;
    private readonly IGuiAndAppHandler<ApplicationModel> _GuiAndAppHandler;

    public DeltaTextHandler(IApplicationModel model, IGuiAndAppHandler<ApplicationModel> guiAndAppHandler) {
        _Model = model;
        _GuiAndAppHandler = guiAndAppHandler;
    }

    public async Task TextChangedAsync(string text) {
        if (_Model.Delta.Text == text) { return; }

        _Model.Delta.Text = text;
        await _GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }
}