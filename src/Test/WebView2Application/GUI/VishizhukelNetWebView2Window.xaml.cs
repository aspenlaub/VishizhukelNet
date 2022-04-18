using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;
using Microsoft.Web.WebView2.Core;
using Moq;
using IApplicationModel = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Interfaces.IApplicationModel;
using IContainer = Autofac.IContainer;
using WindowsApplication = System.Windows.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.GUI {
    // ReSharper disable once UnusedMember.Global
    public partial class VishizhukelNetWebView2Window : IAsyncDisposable {
        private static IContainer Container { get; set; }

        private Application.Application Application;
        private IApplicationModel ApplicationModel;
        private ITashTimer<IApplicationModel> TashTimer;

        public bool NoTash { get; set; }

        public VishizhukelNetWebView2Window() {
            InitializeComponent();
        }

        private async Task BuildContainerIfNecessaryAsync() {
            if (Container != null) { return; }

            var logConfigurationMock = new Mock<ILogConfiguration>();
            logConfigurationMock.SetupGet(lc => lc.LogSubFolder).Returns(@"AspenlaubLogs\" + nameof(VishizhukelNetWebView2Window));
            logConfigurationMock.SetupGet(lc => lc.LogId).Returns($"{DateTime.Today:yyyy-MM-dd}-{Process.GetCurrentProcess().Id}");
            logConfigurationMock.SetupGet(lc => lc.DetailedLogging).Returns(true);
            var builder = await new ContainerBuilder().UseApplicationAsync(this, logConfigurationMock.Object);
            Container = builder.Build();
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e) {
            await BuildContainerIfNecessaryAsync();

            Application = Container.Resolve<Application.Application>();
            ApplicationModel = Container.Resolve<IApplicationModel>();

            const string url = "https://www.viperfisch.de/js/bootstrap.min-v24070.js";
            ApplicationModel.WebView.ScriptToExecuteOnDocumentLoaded
                = $"var script = document.createElement(\"script\"); script.src = \"{url}\"; document.head.appendChild(script);";

            await Application.OnLoadedAsync();

            var commands = Application.Commands;

            var guiToAppGate = Container.Resolve<IGuiToApplicationGate>();
            var buttonNameToCommandMapper = Container.Resolve<IButtonNameToCommandMapper>();

            guiToAppGate.RegisterAsyncTextBoxCallback(WebBrowserOrViewUrl, t => Application.Handlers.WebBrowserUrlTextHandler.TextChangedAsync(t));
            guiToAppGate.RegisterAsyncTextBoxCallback(WebBrowserOrViewContentSource, t => Application.Handlers.WebBrowserContentSourceTextHandler.TextChangedAsync(t));

            guiToAppGate.WireButtonAndCommand(GoToUrl, commands.GoToUrlCommand, buttonNameToCommandMapper);
            guiToAppGate.WireButtonAndCommand(RunJs, commands.RunJsCommand, buttonNameToCommandMapper);

            if (!NoTash) {
                TashTimer = new TashTimer<IApplicationModel>(Container.Resolve<ITashAccessor>(), Application.TashHandler, guiToAppGate);
                if (!await TashTimer.ConnectAndMakeTashRegistrationReturnSuccessAsync(Properties.Resources.WebBrowserWindowTitle)) {
                    Close();
                }

                TashTimer.CreateAndStartTimer(Application.CreateTashTaskHandlingStatus());
            }

            await ExceptionHandler.RunAsync(WindowsApplication.Current, TimeSpan.FromSeconds(5));
        }

        public async ValueTask DisposeAsync() {
            if (TashTimer == null) { return; }

            await TashTimer.StopTimerAndConfirmDeadAsync(false);
        }

        private async void OnClosing(object sender, CancelEventArgs e) {
            if (TashTimer == null) { return; }

            e.Cancel = true;

            await TashTimer.StopTimerAndConfirmDeadAsync(false);

            WindowsApplication.Current.Shutdown();
        }

        private void OnStateChanged(object sender, EventArgs e) {
            Application.OnWindowStateChanged(WindowState);
        }

        private void OnWebViewNavigationStartingAsync(object sender, CoreWebView2NavigationStartingEventArgs e) {
        }

        private async void OnWebViewNavigationCompletedAsync(object sender, CoreWebView2NavigationCompletedEventArgs e) {
            if (Application == null) { return; }

            if (!string.IsNullOrEmpty(ApplicationModel.WebView.ScriptToExecuteOnDocumentLoaded)) {
                await WebView.CoreWebView2.ExecuteScriptAsync(ApplicationModel.WebView.ScriptToExecuteOnDocumentLoaded);
            }

            var source = await WebView.CoreWebView2.ExecuteScriptAsync("document.documentElement.innerHTML");
            source = Regex.Unescape(source);
            source = source.Substring(1, source.Length - 2);
            await Application.OnWebViewNavigationCompletedAsync(source, e.IsSuccess);
        }

        private async void OnWebViewSourceChangedAsync(object sender, CoreWebView2SourceChangedEventArgs e) {
            if (Application == null) { return; }

            await Application.OnWebViewNavigationStartingAsync(WebView.CoreWebView2.Source);
        }
    }
}
