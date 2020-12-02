using System.Runtime.CompilerServices;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Autofac;
using VishizhukelDemoApplication = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application.DemoApplication;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI {
    /// <summary>
    /// Interaction logic for DemoWindow.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class DemoWindow {
        private static IContainer Container { get; set; }

        private VishizhukelDemoApplication vDemoApp;

        public DemoWindow() {
            InitializeComponent();

            var builder = new ContainerBuilder().UseDemoApplication(this);
            Container = builder.Build();

        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private void RegisterTypes() {
            vDemoApp = Container.Resolve<VishizhukelDemoApplication>();
            vDemoApp.RegisterTypes();
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e) {
            RegisterTypes();

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
        }
    }
}
