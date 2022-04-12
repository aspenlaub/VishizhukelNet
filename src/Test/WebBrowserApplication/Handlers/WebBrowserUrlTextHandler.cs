using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Handlers {
    public class WebBrowserUrlTextHandler : ISimpleTextHandler {
        private readonly IApplicationModel Model;
        private readonly IGuiAndAppHandler GuiAndAppHandler;

        public WebBrowserUrlTextHandler(IApplicationModel model, IGuiAndAppHandler guiAndAppHandler) {
            Model = model;
            GuiAndAppHandler = guiAndAppHandler;
        }

        public async Task TextChangedAsync(string text) {
            if (Model.WebBrowserUrl.Text == text) { return; }

            Model.WebBrowserUrl.Text = text;

            await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }
}
