using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Autofac;
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

    private VishizhukelDemoApplication _DemoApp;
    private ITashTimer<ApplicationModel> _TashTimer;

    public bool IsWindowUnderTest { get; set; }

    public VishizhukelNetDemoWindow() {
        InitializeComponent();
    }

    private async Task BuildContainerIfNecessaryAsync() {
        if (Container != null) { return; }

        var builder = await new ContainerBuilder().UseDemoApplicationAsync(this);
        Container = builder.Build();
    }

    private async void OnLoadedAsync(object sender, RoutedEventArgs e) {
        await BuildContainerIfNecessaryAsync();

        _DemoApp = Container.Resolve<VishizhukelDemoApplication>();
        await _DemoApp.OnLoadedAsync();

        var guiToAppGate = Container.Resolve<IGuiToApplicationGate>();
        var buttonNameToCommandMapper = Container.Resolve<IButtonNameToCommandMapper>();
        var toggleButtonNameToHandlerMapper = Container.Resolve<IToggleButtonNameToHandlerMapper>();

        var commands = _DemoApp.Commands;
        guiToAppGate.WireButtonAndCommand(Gamma, commands.GammaCommand, buttonNameToCommandMapper);
        guiToAppGate.WireButtonAndCommand(Iota, commands.IotaCommand, buttonNameToCommandMapper);
        guiToAppGate.WireButtonAndCommand(Kappa, commands.KappaCommand, buttonNameToCommandMapper);

        var handlers = _DemoApp.Handlers;
        guiToAppGate.RegisterAsyncTextBoxCallback(Alpha, _DemoApp.Handlers.AlphaTextHandler.TextChangedAsync);
        guiToAppGate.RegisterAsyncSelectorCallback(Beta, handlers.BetaSelectorHandler.SelectedIndexChangedAsync);

        guiToAppGate.WireToggleButtonAndHandler(MethodAdd, _DemoApp.Handlers.MethodAddHandler, toggleButtonNameToHandlerMapper);
        guiToAppGate.WireToggleButtonAndHandler(MethodMultiply, _DemoApp.Handlers.MethodMultiplyHandler, toggleButtonNameToHandlerMapper);

        guiToAppGate.RegisterAsyncDataGridCallback(Theta, _DemoApp.Handlers.ThetaHandler.CollectionChangedAsync);

        if (IsWindowUnderTest) {
            _TashTimer = new TashTimer<ApplicationModel>(Container.Resolve<ITashAccessor>(), _DemoApp.TashHandler, guiToAppGate);
            if (!await _TashTimer.ConnectAndMakeTashRegistrationReturnSuccessAsync(Properties.Resources.DemoWindowTitle)) {
                Close();
            }

            _TashTimer.CreateAndStartTimer(_DemoApp.CreateTashTaskHandlingStatus());
        }

        AdjustZetaAndItsCanvas();

        await ExceptionHandler.RunAsync(WindowsApplication.Current, TimeSpan.FromSeconds(5));
    }

    public async ValueTask DisposeAsync() {
        if (_TashTimer == null) { return; }

        await _TashTimer.StopTimerAndConfirmDeadAsync(false);
    }

    private async void OnClosing(object sender, CancelEventArgs e) {
        if (_TashTimer == null) { return; }

        e.Cancel = true;

        await _TashTimer.StopTimerAndConfirmDeadAsync(false);

        WindowsApplication.Current.Shutdown();
    }

    private void OnStateChanged(object sender, EventArgs e) {
        _DemoApp.OnWindowStateChanged(WindowState);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
        AdjustZetaAndItsCanvas();
    }

    private void AdjustZetaAndItsCanvas() {
        var adjuster = Container?.Resolve<ICanvasAndImageSizeAdjuster>();
        adjuster?.AdjustCanvasAndImage(ZetaCanvasContainer, ZetaCanvas, Zeta);
    }
}