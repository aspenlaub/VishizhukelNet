﻿using System;
using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;

public abstract class ApplicationBase<TGuiAndApplicationSynchronizer, TModel> : IGuiAndAppHandler
    where TModel : class, IApplicationModelBase
    where TGuiAndApplicationSynchronizer : IGuiAndApplicationSynchronizer<TModel> {
    protected readonly IButtonNameToCommandMapper ButtonNameToCommandMapper;
    protected readonly IToggleButtonNameToHandlerMapper ToggleButtonNameToHandlerMapper;
    protected readonly TGuiAndApplicationSynchronizer GuiAndApplicationSynchronizer;
    protected readonly TModel Model;
    protected readonly IApplicationLogger ApplicationLogger;
    protected readonly IWebViewNavigatingHelper WebViewNavigatingHelper;

    protected ApplicationBase(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
        TGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, TModel model, IApplicationLogger applicationLogger) {
        ButtonNameToCommandMapper = buttonNameToCommandMapper;
        ToggleButtonNameToHandlerMapper = toggleButtonNameToHandlerMapper;
        GuiAndApplicationSynchronizer = guiAndApplicationSynchronizer;
        Model = model;
        ApplicationLogger = applicationLogger;
        WebViewNavigatingHelper = new WebViewNavigatingHelper(model, applicationLogger);
    }

    protected abstract Task EnableOrDisableButtonsAsync();
    protected abstract void CreateCommandsAndHandlers();

    public IApplicationModelBase GetModel() {
        return Model;
    }

    public virtual async Task OnLoadedAsync() {
        CreateCommandsAndHandlers();
        Model.WebViewUrl.Text = "http://localhost/";
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

    public async Task OnWebViewSourceChangedAsync(string uri) {
        ApplicationLogger.LogMessage($"Web view source changes to '{uri}'");
        Model.WebView.IsNavigating = uri != null;
        Model.WebViewUrl.Text = uri ?? "(off road)";
        Model.WebView.LastNavigationStartedAt = DateTime.Now;
        Model.WebViewContentSource.Text = "";
        await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        ApplicationLogger.LogMessage($"GUI navigating to '{Model.WebViewUrl.Text}'");
        IndicateBusy(true);
    }

    public async Task OnWebViewNavigationCompletedAsync(string contentSource, bool isSuccess) {
        ApplicationLogger.LogMessage($"Web view navigation complete: '{Model.WebViewUrl.Text}'");
        Model.WebView.IsNavigating = false;
        Model.WebViewContentSource.Text = contentSource;
        Model.WebView.HasValidDocument = isSuccess;
        if (!isSuccess) {
            ApplicationLogger.LogMessage(Properties.Resources.AppFailed);
            Model.Status.Text = Properties.Resources.CouldNotLoadUrl;
            Model.Status.Type = StatusType.Error;
        }

        await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        IndicateBusy(true);
    }

    public async Task<TResult> RunScriptAsync<TResult>(IScriptStatement scriptStatement, bool mayFail, bool maySucceed) where TResult : IScriptCallResponse, new() {
        var scriptCallResponse = await GuiAndApplicationSynchronizer.RunScriptAsync<TResult>(scriptStatement);

        if (scriptCallResponse.Success.Inconclusive) {
            Model.Status.Text = string.IsNullOrEmpty(scriptStatement.InconclusiveErrorMessage) ? scriptStatement.NoSuccessErrorMessage : scriptStatement.InconclusiveErrorMessage;
            Model.Status.Type = StatusType.Error;
            return scriptCallResponse;
        }

        if (scriptCallResponse.Success.YesNo && maySucceed || !scriptCallResponse.Success.YesNo && mayFail) {
            return scriptCallResponse;
        }

        Model.Status.Text = scriptCallResponse.Success.YesNo ? scriptStatement.NoFailureErrorMessage : scriptStatement.NoSuccessErrorMessage;
        Model.Status.Type = StatusType.Error;
        return scriptCallResponse;
    }
}