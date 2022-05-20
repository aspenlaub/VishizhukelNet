using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Handlers;

internal class WebViewContentSourceTextHandler : ISimpleTextHandler {
    private readonly IApplicationModel Model;
    private readonly IGuiAndAppHandler GuiAndAppHandler;

    public WebViewContentSourceTextHandler(IApplicationModel model, IGuiAndAppHandler guiAndAppHandler) {
        Model = model;
        GuiAndAppHandler = guiAndAppHandler;
    }

    public async Task TextChangedAsync(string text) {
        if (Model.WebViewContentSource.Text == text) { return; }

        Model.WebViewContentSource.Text = text;

        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }
}