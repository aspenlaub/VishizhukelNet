using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Handlers;

internal class WebBrowserContentSourceTextHandler : ISimpleTextHandler {
    private readonly IApplicationModel Model;
    private readonly IGuiAndAppHandler GuiAndAppHandler;

    public WebBrowserContentSourceTextHandler(IApplicationModel model, IGuiAndAppHandler guiAndAppHandler) {
        Model = model;
        GuiAndAppHandler = guiAndAppHandler;
    }

    public async Task TextChangedAsync(string text) {
        if (Model.WebBrowserOrViewContentSource.Text == text) { return; }

        Model.WebBrowserOrViewContentSource.Text = text;

        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }
}