using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;

public class WebBrowserOrViewNavigationHelper<TApplicationModel> : IWebBrowserOrViewNavigationHelper where TApplicationModel : IApplicationModelBase {
    private readonly TApplicationModel Model;
    private readonly IApplicationLogger ApplicationLogger;
    private readonly IGuiAndAppHandler GuiAndAppHandler;
    private readonly IWebBrowserOrViewNavigatingHelper WebBrowserOrViewNavigatingHelper;

    public WebBrowserOrViewNavigationHelper(TApplicationModel model, IApplicationLogger applicationLogger, IGuiAndAppHandler guiAndAppHandler, IWebBrowserOrViewNavigatingHelper webBrowserOrViewNavigatingHelper) {
        Model = model;
        ApplicationLogger = applicationLogger;
        GuiAndAppHandler = guiAndAppHandler;
        WebBrowserOrViewNavigatingHelper = webBrowserOrViewNavigatingHelper;
    }

    public async Task<bool> NavigateToUrlAsync(string url) {
        ApplicationLogger.LogMessage($"App navigating to '{url}'");

        if (!await WebBrowserOrViewNavigatingHelper.WaitUntilNotNavigatingAnymoreAsync(url, DateTime.MinValue)) {
            return false;
        }

        ApplicationLogger.LogMessage("Reset model url and sync");
        Model.WebBrowserOrView.Url = Urls.AboutBlank;
        var minLastUpdateTime = DateTime.Now;
        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();

        if (Model.UsesRealBrowserOrView) {
            if (!await WebBrowserOrViewNavigatingHelper.WaitUntilNotNavigatingAnymoreAsync(url, minLastUpdateTime)) {
                return false;
            }
        }

        ApplicationLogger.LogMessage("Set model url and sync");
        Model.WebBrowserOrView.Url = url;
        minLastUpdateTime = DateTime.Now;
        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();

        if (Model.UsesRealBrowserOrView) {
            if (!await WebBrowserOrViewNavigatingHelper.WaitUntilNotNavigatingAnymoreAsync(url, minLastUpdateTime)) {
                return false;
            }
        } else {
            Model.WebBrowserOrView.RevalidateDocument();

            if (Model.WebBrowserOrView.HasValidDocument) {
                Model.Status.Text = "";
                Model.Status.Type = StatusType.None;
                return true;
            }

            ApplicationLogger.LogMessage("App failed");
            Model.Status.Text = Properties.Resources.CouldNotLoadUrl;
            Model.Status.Type = StatusType.Error;
            return false;
        }

        Model.Status.Text = "";
        Model.Status.Type = StatusType.None;
        return true;
    }
}