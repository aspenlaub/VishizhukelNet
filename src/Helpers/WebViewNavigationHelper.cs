using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;

public class WebViewNavigationHelper<TApplicationModel> : IWebViewNavigationHelper where TApplicationModel : IApplicationModelBase {
    private readonly TApplicationModel Model;
    private readonly IApplicationLogger ApplicationLogger;
    private readonly IGuiAndAppHandler GuiAndAppHandler;
    private readonly IWebViewNavigatingHelper WebViewNavigatingHelper;

    public WebViewNavigationHelper(TApplicationModel model, IApplicationLogger applicationLogger, IGuiAndAppHandler guiAndAppHandler, IWebViewNavigatingHelper webViewNavigatingHelper) {
        Model = model;
        ApplicationLogger = applicationLogger;
        GuiAndAppHandler = guiAndAppHandler;
        WebViewNavigatingHelper = webViewNavigatingHelper;
    }

    public async Task<bool> NavigateToUrlAsync(string url) {
        ApplicationLogger.LogMessage($"App navigating to '{url}'");

        if (!await WebViewNavigatingHelper.WaitUntilNotNavigatingAnymoreAsync(url, DateTime.MinValue)) {
            return false;
        }

        ApplicationLogger.LogMessage(Properties.Resources.ResetModelUrlAndSync);
        Model.WebView.Url = Urls.AboutBlank;
        var minLastUpdateTime = DateTime.Now;
        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();

        if (Model.UsesRealBrowserOrView) {
            if (!await WebViewNavigatingHelper.WaitUntilNotNavigatingAnymoreAsync(url, minLastUpdateTime)) {
                return false;
            }
        }

        ApplicationLogger.LogMessage(Properties.Resources.SetModelUrlAndAsync);
        Model.WebView.Url = url;
        minLastUpdateTime = DateTime.Now;
        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();

        if (Model.UsesRealBrowserOrView) {
            if (!await WebViewNavigatingHelper.WaitUntilNotNavigatingAnymoreAsync(url, minLastUpdateTime)) {
                return false;
            }
        } else {
            if (Model.WebView.HasValidDocument) {
                Model.Status.Text = "";
                Model.Status.Type = StatusType.None;
                return true;
            }

            ApplicationLogger.LogMessage(Properties.Resources.AppFailed);
            Model.Status.Text = Properties.Resources.CouldNotLoadUrl;
            Model.Status.Type = StatusType.Error;
            return false;
        }

        Model.Status.Text = "";
        Model.Status.Type = StatusType.None;
        return true;
    }
}