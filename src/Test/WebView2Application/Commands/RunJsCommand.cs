using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Commands {
    public class RunJsCommand : ICommand {
        private readonly IApplicationModel Model;
        private readonly IGuiAndAppHandler GuiAndAppHandler;

        public RunJsCommand(IApplicationModel model, IGuiAndAppHandler guiAndAppHandler) {
            Model = model;
            GuiAndAppHandler = guiAndAppHandler;
        }

        public async Task ExecuteAsync() {
            if (!Model.RunJs.Enabled) {
                return;
            }

            Model.WebView.ScriptCodeToExecute = "alert('A script has been run');";

            await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();

        }

        public async Task<bool> ShouldBeEnabledAsync() {
            var enabled = Model.WebBrowserOrViewUrl.Text.StartsWith("http", StringComparison.InvariantCulture);
            return await Task.FromResult(enabled);
        }
    }
}
