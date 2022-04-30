using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Commands;

public class GoToUrlCommand : ICommand {
    private readonly IApplicationModel Model;
    private readonly IWebBrowserOrViewNavigationHelper WebBrowserOrViewNavigationHelper;

    public GoToUrlCommand(IApplicationModel model, IWebBrowserOrViewNavigationHelper webBrowserOrViewNavigationHelper) {
        Model = model;
        WebBrowserOrViewNavigationHelper = webBrowserOrViewNavigationHelper;
    }

    public async Task ExecuteAsync() {
        if (!Model.GoToUrl.Enabled) {
            return;
        }

        await WebBrowserOrViewNavigationHelper.NavigateToUrlAsync(Model.WebBrowserOrViewUrl.Text);
    }

    public async Task<bool> ShouldBeEnabledAsync() {
        var enabled = Model.WebBrowserOrViewUrl.Text.StartsWith("http", StringComparison.InvariantCulture);
        return await Task.FromResult(enabled);
    }
}