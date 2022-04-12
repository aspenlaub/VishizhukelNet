using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;
using Moq;
using IApplicationModel = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces.IApplicationModel;
using IContainer = Autofac.IContainer;
using WindowsApplication = System.Windows.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.GUI {
    // ReSharper disable once UnusedMember.Global
    public partial class VishizhukelNetWebBrowserWindow : IAsyncDisposable {
        private static IContainer Container { get; set; }

        private Application.Application Application;
        private ITashTimer<IApplicationModel> TashTimer;

        public bool NoTash { get; set; }

        public VishizhukelNetWebBrowserWindow() {
            InitializeComponent();
        }

        private async Task BuildContainerIfNecessaryAsync() {
            if (Container != null) { return; }

            var logConfigurationMock = new Mock<ILogConfiguration>();
            logConfigurationMock.SetupGet(lc => lc.LogSubFolder).Returns(@"AspenlaubLogs\" + nameof(VishizhukelNetWebBrowserWindow));
            logConfigurationMock.SetupGet(lc => lc.LogId).Returns($"{DateTime.Today:yyyy-MM-dd}-{Process.GetCurrentProcess().Id}");
            logConfigurationMock.SetupGet(lc => lc.DetailedLogging).Returns(true);
            var builder = await new ContainerBuilder().UseApplicationAsync(this, logConfigurationMock.Object);
            Container = builder.Build();
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e) {
            await BuildContainerIfNecessaryAsync();

            Application = Container.Resolve<Application.Application>();
            await Application.OnLoadedAsync();

            var commands = Application.Commands;

            var guiToAppGate = Container.Resolve<IGuiToApplicationGate>();
            var buttonNameToCommandMapper = Container.Resolve<IButtonNameToCommandMapper>();

            guiToAppGate.RegisterAsyncTextBoxCallback(WebBrowserUrl, t => Application.Handlers.WebBrowserUrlTextHandler.TextChangedAsync(t));

            guiToAppGate.WireButtonAndCommand(GoToUrl, commands.GoToUrlCommand, buttonNameToCommandMapper);

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

        private void OnWebBrowserNavigating(object sender, NavigatingCancelEventArgs e) {
            Application.OnWebBrowserNavigating(e.Uri);
        }

        private void OnWebBrowserLoadCompleted(object sender, NavigationEventArgs e) {
            Application.OnWebBrowserLoadCompleted(((WebBrowser)sender).Document);
        }

        private void OnWebBrowserNavigated(object sender, NavigationEventArgs e) {
        }
    }
}
