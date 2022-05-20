using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Handlers;

public class WebViewUrlTextHandler : ISimpleTextHandler {
    private readonly IApplicationModel Model;
    private readonly IGuiAndAppHandler GuiAndAppHandler;

    public WebViewUrlTextHandler(IApplicationModel model, IGuiAndAppHandler guiAndAppHandler) {
        Model = model;
        GuiAndAppHandler = guiAndAppHandler;
    }

    public async Task TextChangedAsync(string text) {
        if (Model.WebViewUrl.Text == text) { return; }

        Model.WebViewUrl.Text = text;

        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }
}