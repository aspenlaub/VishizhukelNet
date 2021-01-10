using System;
using System.ComponentModel;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Autofac;
using IContainer = Autofac.IContainer;
using VishizhukelDemoApplication = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application.DemoApplication;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI {
    /// <summary>
    /// Interaction logic for DemoWindow.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class DemoWindow : IDisposable {
        private static IContainer Container { get; set; }

        private VishizhukelDemoApplication vDemoApp;
        private ITashTimer<IDemoApplicationModel> vTashTimer;

        public DemoWindow() {
            InitializeComponent();

            var builder = new ContainerBuilder().UseDemoApplication(this);
            Container = builder.Build();

        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e) {
            vDemoApp = Container.Resolve<VishizhukelDemoApplication>();
            await vDemoApp.OnLoadedAsync();

            var guiToAppGate = Container.Resolve<IGuiToApplicationGate>();
            var buttonNameToCommandMapper = Container.Resolve<IButtonNameToCommandMapper>();

            var commands = vDemoApp.Commands;
            guiToAppGate.WireButtonAndCommand(Gamma, commands.GammaCommand, buttonNameToCommandMapper);

            var handlers = vDemoApp.Handlers;
            guiToAppGate.RegisterAsyncTextBoxCallback(Alpha, t => vDemoApp.Handlers.AlphaTextHandler.TextChangedAsync(t));
            guiToAppGate.RegisterAsyncSelectorCallback(Beta, i => handlers.BetaSelectorHandler.SelectedIndexChangedAsync(i));
            guiToAppGate.RegisterAsyncToggleButtonCallback(MethodAdd, b => vDemoApp.Handlers.MethodAddHandler.ToggledAsync(b));
            guiToAppGate.RegisterAsyncToggleButtonCallback(MethodMultiply, b => vDemoApp.Handlers.MethodMultiplyHandler.ToggledAsync(b));

            vTashTimer = new TashTimer<IDemoApplicationModel>(Container.Resolve<ITashAccessor>(), vDemoApp.TashHandler, guiToAppGate);
            if (!await vTashTimer.ConnectAndMakeTashRegistrationReturnSuccessAsync(Properties.Resources.DemoWindowTitle)) {
                Close();
            }

            vTashTimer.CreateAndStartTimer(vDemoApp.CreateTashTaskHandlingStatus());
        }

        public void Dispose() {
            vTashTimer?.StopTimerAndConfirmDead(false);
        }

        private void OnClosing(object sender, CancelEventArgs e) {
            vTashTimer?.StopTimerAndConfirmDead(false);
        }
    }
}
