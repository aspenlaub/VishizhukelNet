using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Entities;
using Autofac;
using Moq;
using IContainer = Autofac.IContainer;
using WindowsApplication = System.Windows.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.GUI;

// ReSharper disable once UnusedMember.Global
public partial class VishizhukelNetEmptyWindow : IAsyncDisposable, IVishizhukelNetWindowUnderTest {
    private static IContainer Container { get; set; }

    private Application.Application Application;
    private ITashTimer<ApplicationModel> TashTimer;

    public bool IsWindowUnderTest { get; set; }

    public VishizhukelNetEmptyWindow() {
        InitializeComponent();
    }

    private async Task BuildContainerIfNecessaryAsync() {
        if (Container != null) { return; }

        var builder = await new ContainerBuilder().UseApplicationAsync(this);
        Container = builder.Build();
    }

    private async void OnLoadedAsync(object sender, RoutedEventArgs e) {
        await BuildContainerIfNecessaryAsync();

        Application = Container.Resolve<Application.Application>();
        await Application.OnLoadedAsync();

        if (IsWindowUnderTest) {
            var guiToAppGate = Container.Resolve<IGuiToApplicationGate>();
            TashTimer = new TashTimer<ApplicationModel>(Container.Resolve<ITashAccessor>(), Application.TashHandler, guiToAppGate);
            if (!await TashTimer.ConnectAndMakeTashRegistrationReturnSuccessAsync(Properties.Resources.EmptyWindowTitle)) {
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
}