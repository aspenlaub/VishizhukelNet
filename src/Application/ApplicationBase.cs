﻿using System;
using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using MSHTML;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;

public abstract class ApplicationBase<TGuiAndApplicationSynchronizer, TModel> : IGuiAndAppHandler
    where TModel : class, IApplicationModelBase
    where TGuiAndApplicationSynchronizer : IGuiAndApplicationSynchronizer<TModel> {
    protected readonly IButtonNameToCommandMapper ButtonNameToCommandMapper;
    protected readonly IToggleButtonNameToHandlerMapper ToggleButtonNameToHandlerMapper;
    protected readonly TGuiAndApplicationSynchronizer GuiAndApplicationSynchronizer;
    protected readonly TModel Model;
    protected readonly IBasicHtmlHelper BasicHtmlHelper;
    protected readonly IApplicationLogger ApplicationLogger;
    protected readonly IWebBrowserOrViewNavigatingHelper WebBrowserOrViewNavigatingHelper;

    protected ApplicationBase(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
        TGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, TModel model, IBasicHtmlHelper basicHtmlHelper, IApplicationLogger applicationLogger) {
        ButtonNameToCommandMapper = buttonNameToCommandMapper;
        ToggleButtonNameToHandlerMapper = toggleButtonNameToHandlerMapper;
        GuiAndApplicationSynchronizer = guiAndApplicationSynchronizer;
        Model = model;
        BasicHtmlHelper = basicHtmlHelper;
        ApplicationLogger = applicationLogger;
        WebBrowserOrViewNavigatingHelper = new WebBrowserOrViewNavigatingHelper(model, applicationLogger);
    }

    protected abstract Task EnableOrDisableButtonsAsync();
    protected abstract void CreateCommandsAndHandlers();

    public IApplicationModelBase GetModel() {
        return Model;
    }

    public virtual async Task OnLoadedAsync() {
        CreateCommandsAndHandlers();
        Model.WebBrowserOrViewUrl.Text = "http://localhost/";
        await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }

    public async Task EnableOrDisableButtonsThenSyncGuiAndAppAsync() {
        await EnableOrDisableButtonsAsync();
        await GuiAndApplicationSynchronizer.OnModelDataChangedAsync();
    }

    public async Task SyncGuiAndAppAsync() {
        await GuiAndApplicationSynchronizer.OnModelDataChangedAsync();
    }

    public void IndicateBusy(bool force) {
        GuiAndApplicationSynchronizer.IndicateBusy(force);
    }

    public void OnWindowStateChanged(WindowState windowState) {
        Model.WindowState = windowState;
    }

    public async Task OnWebBrowserNavigatingAsync(Uri uri) {
        ApplicationLogger.LogMessage($"Web browser navigating to '{Model.WebBrowserOrViewUrl.Text}'");
        Model.WebBrowser.IsNavigating = uri != null;
        Model.WebBrowserOrViewUrl.Text = uri == null ? "(off road)" : uri.OriginalString;
        Model.WebBrowser.Document = null;
        Model.WebBrowser.LastNavigationStartedAt = DateTime.Now;
        Model.WebBrowserOrViewContentSource.Text = "";
        await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        IndicateBusy(true);
    }

    public async Task OnWebBrowserLoadCompletedAsync(object documentObject) {
        var document = BasicHtmlHelper.ObjectAsDocument(documentObject);
        await OnWebBrowserLoadCompletedAsync(document, BasicHtmlHelper.DocumentToHtml(document));
    }

    public async Task OnWebBrowserLoadCompletedAsync(IHTMLDocument3 document, string documentAsString) {
        ApplicationLogger.LogMessage($"Web browser navigation complete: '{Model.WebBrowserOrViewUrl.Text}'");
        Model.WebBrowser.Document = document;
        Model.WebBrowser.IsNavigating = false;
        Model.WebBrowser.RevalidateDocument();
        GuiAndApplicationSynchronizer.OnWebBrowserLoadCompleted();
        Model.WebBrowserOrViewContentSource.Text = documentAsString;
        await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        IndicateBusy(true);
    }

    public async Task OnWebViewSourceChangedAsync(string uri) {
        ApplicationLogger.LogMessage($"Web view source changes to '{uri}'");
        Model.WebView.IsNavigating = uri != null;
        Model.WebBrowserOrViewUrl.Text = uri ?? "(off road)";
        Model.WebView.LastNavigationStartedAt = DateTime.Now;
        Model.WebBrowserOrViewContentSource.Text = "";
        await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        ApplicationLogger.LogMessage($"GUI navigating to '{Model.WebBrowserOrViewUrl.Text}'");
        IndicateBusy(true);
    }

    public async Task OnWebViewNavigationCompletedAsync(string contentSource, bool isSuccess) {
        ApplicationLogger.LogMessage($"Web view navigation complete: '{Model.WebBrowserOrViewUrl.Text}'");
        Model.WebView.IsNavigating = false;
        Model.WebBrowserOrViewContentSource.Text = contentSource;
        Model.WebView.HasValidDocument = isSuccess;
        if (!isSuccess) {
            ApplicationLogger.LogMessage("App failed");
            Model.Status.Text = Properties.Resources.CouldNotLoadUrl;
            Model.Status.Type = StatusType.Error;
        }

        await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        IndicateBusy(true);
    }
}