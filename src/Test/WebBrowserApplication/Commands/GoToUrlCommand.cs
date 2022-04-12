using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Commands {
    public class GoToUrlCommand : ICommand {
        private readonly IApplicationModel Model;
        private readonly IWebBrowserNavigationHelper WebBrowserNavigationHelper;

        public GoToUrlCommand(IApplicationModel model, IWebBrowserNavigationHelper webBrowserNavigationHelper) {
            Model = model;
            WebBrowserNavigationHelper = webBrowserNavigationHelper;
        }

        public async Task ExecuteAsync() {
            if (!Model.GoToUrl.Enabled) {
                return;
            }

            await WebBrowserNavigationHelper.NavigateToUrlAsync(Model.WebBrowserUrl.Text);
        }

        public async Task<bool> ShouldBeEnabledAsync() {
            var enabled = Model.WebBrowserUrl.Text.StartsWith("http", StringComparison.InvariantCulture);
            return await Task.FromResult(enabled);
        }
    }
}