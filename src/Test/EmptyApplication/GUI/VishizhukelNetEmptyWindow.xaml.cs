using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;
using Moq;
using IApplicationModel = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Interfaces.IApplicationModel;
using IContainer = Autofac.IContainer;
using VishizhukelApplication = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Application.Application;
using WindowsApplication = System.Windows.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.GUI {
    // ReSharper disable once UnusedMember.Global
    public partial class VishizhukelNetEmptyWindow : IAsyncDisposable {
        private static IContainer Container { get; set; }

        private VishizhukelApplication Application;
        private ITashTimer<IApplicationModel> TashTimer;

        public VishizhukelNetEmptyWindow() {
            InitializeComponent();
        }

        private async Task BuildContainerIfNecessaryAsync() {
            if (Container != null) { return; }

            var logConfigurationMock = new Mock<ILogConfiguration>();
            logConfigurationMock.SetupGet(lc => lc.LogSubFolder).Returns(@"AspenlaubLogs\" + nameof(VishizhukelNetEmptyWindow));
            logConfigurationMock.SetupGet(lc => lc.LogId).Returns($"{DateTime.Today:yyyy-MM-dd}-{Process.GetCurrentProcess().Id}");
            logConfigurationMock.SetupGet(lc => lc.DetailedLogging).Returns(true);
            var builder = await new ContainerBuilder().UseApplicationAsync(this, logConfigurationMock.Object);
            Container = builder.Build();
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e) {
            await BuildContainerIfNecessaryAsync();

            Application = Container.Resolve<VishizhukelApplication>();
            await Application.OnLoadedAsync();

            var guiToAppGate = Container.Resolve<IGuiToApplicationGate>();
            TashTimer = new TashTimer<IApplicationModel>(Container.Resolve<ITashAccessor>(), Application.TashHandler, guiToAppGate);
            if (!await TashTimer.ConnectAndMakeTashRegistrationReturnSuccessAsync(Properties.Resources.EmptyWindowTitle)) {
                Close();
            }

            TashTimer.CreateAndStartTimer(Application.CreateTashTaskHandlingStatus());

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
    }
}
