using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Extensions;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Button = System.Windows.Controls.Button;
using Selector = System.Windows.Controls.Primitives.Selector;
using TextBox = System.Windows.Controls.TextBox;
using ToggleButton = System.Windows.Controls.Primitives.ToggleButton;

#nullable disable

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;

public abstract class GuiToApplicationGateBase<TApplication> : IGuiToApplicationGate where TApplication : class, IGuiAndAppHandler {
    protected readonly IBusy Busy;
    protected readonly TApplication Application;
    protected readonly IApplicationModelBase ApplicationModel;

    protected GuiToApplicationGateBase(IBusy busy, TApplication application) {
        Busy = busy;
        Application = application;
        ApplicationModel = Application.GetModel();
    }

    public async Task CallbackAsync(Func<Task> action) {
        if (Busy.IsBusy) { return; }

        Busy.IsBusy = true;
        await action();
        Busy.IsBusy = false;
        await Application.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }

    public void RegisterAsyncButtonCallback(Button button, Func<Task> action) {
        button.Click += async (_, _) => await CallbackAsync(() => action());
    }

    public void RegisterAsyncTextBoxCallback(TextBox textBox, Func<string, Task> action) {
        textBox.TextChanged += async (_, _) => await CallbackAsync(() => action(textBox.Text));
    }

    public void RegisterAsyncSelectorCallback(Selector selector, Func<int, Task> action) {
        selector.SelectionChanged += async (_, _) => await CallbackAsync(() => action(selector.SelectedIndex));
    }

    public void WireToggleButtonAndHandler(ToggleButton toggleButton, IToggleButtonHandler handler, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper) {
        toggleButtonNameToHandlerMapper.Register(toggleButton.Name, handler);
        toggleButton.Click += async (_, _) => await CallbackAsync(() => handler.ToggledAsync(toggleButton.IsChecked == true));
    }

    public void WireButtonAndCommand(Button button, ICommand command, IButtonNameToCommandMapper buttonNameToCommandMapper) {
        buttonNameToCommandMapper.Register(button.Name, command);
        RegisterAsyncButtonCallback(button, command.ExecuteAsync);
    }

    public void RegisterAsyncDataGridCallback(DataGrid dataGrid, Func<IList<ICollectionViewSourceEntity>, Task> action) {
        dataGrid.CurrentCellChanged += async (_, _) => await CallbackAsync(() => {
            var items = dataGrid.Items.OfType<ICollectionViewSourceEntity>().ToList();
            return action(items);
        });
    }

    public void WireWebView(WebView2 webView) {
        webView.SourceChanged += OnWebViewOnSourceChangedAsync;
        webView.NavigationStarting += OnNavigationStartingAsync;
        webView.NavigationCompleted += OnNavigationCompletedAsync;
        ApplicationModel.WebView.IsWired = true;
    }

    private async void OnNavigationStartingAsync(object sender, CoreWebView2NavigationStartingEventArgs e) {
        if (Application == null) { return; }

        var webView = sender as WebView2;
        if (webView == null) { return; }

        await Application.OnWebViewSourceChangedAsync(e.Uri);
    }

    private async void OnWebViewOnSourceChangedAsync(object sender, CoreWebView2SourceChangedEventArgs e) {
        if (Application == null) { return; }

        var webView = sender as WebView2;
        if (webView == null) { return; }

        if (ApplicationModel.WebView.IsNavigating && ApplicationModel.WebView.Url == webView.CoreWebView2.Source) {
            return;
        }

        await Application.OnWebViewSourceChangedAsync(webView.CoreWebView2.Source);
    }

    private async void OnNavigationCompletedAsync(object sender, CoreWebView2NavigationCompletedEventArgs e) {
        if (Application == null) { return; }

        var webView = sender as WebView2;
        if (webView == null) { return; }

        if (ApplicationModel.WebView.OnDocumentLoaded.Any()) {
            await webView.CoreWebView2.ExecuteScriptAsync(ApplicationModel.WebView.OnDocumentLoaded.Statement);
        }

        var source = await webView.CoreWebView2.ExecuteScriptAsync("document.documentElement.innerHTML");
        source = Regex.Unescape(source);
        source = source.Substring(1, source.Length - 2);
        await Application.OnWebViewNavigationCompletedAsync(source, e.IsSuccess);
    }
}