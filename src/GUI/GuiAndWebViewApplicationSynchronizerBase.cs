using System;
using System.Reflection;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Web.WebView2.Wpf;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;

public class GuiAndWebViewApplicationSynchronizerBase<TModel, TWindow>
    : GuiAndApplicationSynchronizerBase<TModel, TWindow>, IGuiAndWebViewApplicationSynchronizer<TModel>
    where TModel : class, IWebViewApplicationModelBase {
    protected readonly IWebViewNavigatingHelper WebViewNavigatingHelper;

    protected GuiAndWebViewApplicationSynchronizerBase(TModel model, TWindow window, IApplicationLogger applicationLogger) : base(model, window, applicationLogger) {
        WebViewNavigatingHelper = new WebViewNavigatingHelper(Model, ApplicationLogger);
    }

    protected override async Task UpdateFieldIfNecessaryAsync(FieldInfo windowField, PropertyInfo modelProperty) {
        switch (windowField.FieldType.Name) {
            case "WebView2":
                await UpdateWebViewIfNecessaryAsync((IWebView)modelProperty.GetValue(Model),
                    (WebView2)windowField.GetValue(Window));
                break;
            default:
                await base.UpdateFieldIfNecessaryAsync(windowField, modelProperty);
                break;
        }
    }

    private async Task UpdateWebViewIfNecessaryAsync(IWebView modelWebView, WebView2 webView2) {
        if (modelWebView == null) {
            throw new ArgumentNullException(nameof(modelWebView));
        }

        if (!modelWebView.IsWired || modelWebView.Url == modelWebView.LastUrl) {
            return;
        }

        modelWebView.LastUrl = modelWebView.Url;
        ApplicationLogger.LogMessage($"Calling webView2.CoreWebView2.Navigate with '{modelWebView.Url}'");
        var minLastUpdateTime = DateTime.Now;
        webView2.CoreWebView2?.Navigate(modelWebView.Url);

        await WebViewNavigatingHelper.WaitUntilNotNavigatingAnymoreAsync(modelWebView.LastUrl, minLastUpdateTime);
    }

    public async Task WaitUntilNotNavigatingAnymoreAsync() {
        await WebViewNavigatingHelper.WaitUntilNotNavigatingAnymoreAsync("", DateTime.MinValue);
    }
}