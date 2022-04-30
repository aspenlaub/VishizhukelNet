using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Autofac;
using Moq;
using IContainer = Autofac.IContainer;
using VishizhukelDemoApplication = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application.Application;
using WindowsApplication = System.Windows.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;

/// <summary>
/// Interaction logic for VishizhukelNetDemoWindow.xaml
/// </summary>
// ReSharper disable once UnusedMember.Global
public partial class VishizhukelNetDemoWindow : IAsyncDisposable, IVishizhukelNetWindowUnderTest {
    private static IContainer Container { get; set; }

    private VishizhukelDemoApplication DemoApp;
    private ITashTimer<IApplicationModel> TashTimer;

    public bool IsWindowUnderTest { get; set; }

    public VishizhukelNetDemoWindow() {
        InitializeComponent();
    }

    private async Task BuildContainerIfNecessaryAsync() {
        if (Container != null) { return; }

        var logConfigurationMock = new Mock<ILogConfiguration>();
        logConfigurationMock.SetupGet(lc => lc.LogSubFolder).Returns(@"AspenlaubLogs\" + nameof(VishizhukelNetDemoWindow));
        logConfigurationMock.SetupGet(lc => lc.LogId).Returns($"{DateTime.Today:yyyy-MM-dd}-{Process.GetCurrentProcess().Id}");
        logConfigurationMock.SetupGet(lc => lc.DetailedLogging).Returns(true);
        var builder = await new ContainerBuilder().UseDemoApplicationAsync(this, logConfigurationMock.Object);
        Container = builder.Build();
    }

    private async void OnLoadedAsync(object sender, RoutedEventArgs e) {
        await BuildContainerIfNecessaryAsync();

        DemoApp = Container.Resolve<VishizhukelDemoApplication>();
        await DemoApp.OnLoadedAsync();

        var guiToAppGate = Container.Resolve<IGuiToApplicationGate>();
        var buttonNameToCommandMapper = Container.Resolve<IButtonNameToCommandMapper>();
        var toggleButtonNameToHandlerMapper = Container.Resolve<IToggleButtonNameToHandlerMapper>();

        var commands = DemoApp.Commands;
        guiToAppGate.WireButtonAndCommand(Gamma, commands.GammaCommand, buttonNameToCommandMapper);
        guiToAppGate.WireButtonAndCommand(Iota, commands.IotaCommand, buttonNameToCommandMapper);
        guiToAppGate.WireButtonAndCommand(Kappa, commands.KappaCommand, buttonNameToCommandMapper);

        var handlers = DemoApp.Handlers;
        guiToAppGate.RegisterAsyncTextBoxCallback(Alpha, t => DemoApp.Handlers.AlphaTextHandler.TextChangedAsync(t));
        guiToAppGate.RegisterAsyncSelectorCallback(Beta, i => handlers.BetaSelectorHandler.SelectedIndexChangedAsync(i));

        guiToAppGate.WireToggleButtonAndHandler(MethodAdd, DemoApp.Handlers.MethodAddHandler, toggleButtonNameToHandlerMapper);
        guiToAppGate.WireToggleButtonAndHandler(MethodMultiply, DemoApp.Handlers.MethodMultiplyHandler, toggleButtonNameToHandlerMapper);

        guiToAppGate.RegisterAsyncDataGridCallback(Theta, items => DemoApp.Handlers.ThetaHandler.CollectionChangedAsync(items));

        if (IsWindowUnderTest) {
            TashTimer = new TashTimer<IApplicationModel>(Container.Resolve<ITashAccessor>(), DemoApp.TashHandler, guiToAppGate);
            if (!await TashTimer.ConnectAndMakeTashRegistrationReturnSuccessAsync(Properties.Resources.DemoWindowTitle)) {
                Close();
            }

            TashTimer.CreateAndStartTimer(DemoApp.CreateTashTaskHandlingStatus());
        }

        AdjustZetaAndItsCanvas();

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
        DemoApp.OnWindowStateChanged(WindowState);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
        AdjustZetaAndItsCanvas();
    }

    private void AdjustZetaAndItsCanvas() {
        var adjuster = Container?.Resolve<ICanvasAndImageSizeAdjuster>();
        adjuster?.AdjustCanvasAndImage(ZetaCanvasContainer, ZetaCanvas, Zeta);
    }
}