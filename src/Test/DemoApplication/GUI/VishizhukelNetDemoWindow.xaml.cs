﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Autofac;
using Moq;
using IContainer = Autofac.IContainer;
using VishizhukelDemoApplication = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application.DemoApplication;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI {
    /// <summary>
    /// Interaction logic for VishizhukelNetDemoWindow.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class VishizhukelNetDemoWindow : IDisposable {
        private static IContainer Container { get; set; }

        private VishizhukelDemoApplication vDemoApp;
        private ITashTimer<IDemoApplicationModel> vTashTimer;

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

            vDemoApp = Container.Resolve<VishizhukelDemoApplication>();
            await vDemoApp.OnLoadedAsync();

            var guiToAppGate = Container.Resolve<IGuiToApplicationGate>();
            var buttonNameToCommandMapper = Container.Resolve<IButtonNameToCommandMapper>();
            var toggleButtonNameToHandlerMapper = Container.Resolve<IToggleButtonNameToHandlerMapper>();

            var commands = vDemoApp.Commands;
            guiToAppGate.WireButtonAndCommand(Gamma, commands.GammaCommand, buttonNameToCommandMapper);

            var handlers = vDemoApp.Handlers;
            guiToAppGate.RegisterAsyncTextBoxCallback(Alpha, t => vDemoApp.Handlers.AlphaTextHandler.TextChangedAsync(t));
            guiToAppGate.RegisterAsyncSelectorCallback(Beta, i => handlers.BetaSelectorHandler.SelectedIndexChangedAsync(i));

            guiToAppGate.WireToggleButtonAndHandler(MethodAdd, vDemoApp.Handlers.MethodAddHandler, toggleButtonNameToHandlerMapper);
            guiToAppGate.WireToggleButtonAndHandler(MethodMultiply, vDemoApp.Handlers.MethodMultiplyHandler, toggleButtonNameToHandlerMapper);

            guiToAppGate.RegisterAsyncDataGridCallback(Theta, items => vDemoApp.Handlers.ThetaHandler.CollectionChangedAsync(items));

            vTashTimer = new TashTimer<IDemoApplicationModel>(Container.Resolve<ITashAccessor>(), vDemoApp.TashHandler, guiToAppGate);
            if (!await vTashTimer.ConnectAndMakeTashRegistrationReturnSuccessAsync(Properties.Resources.DemoWindowTitle)) {
                Close();
            }

            vTashTimer.CreateAndStartTimer(vDemoApp.CreateTashTaskHandlingStatus());

            AdjustZetaAndItsCanvas();
        }

        public void Dispose() {
            vTashTimer?.StopTimerAndConfirmDead(false);
        }

        private void OnClosing(object sender, CancelEventArgs e) {
            vTashTimer?.StopTimerAndConfirmDead(false);
        }

        private void OnStateChanged(object sender, EventArgs e) {
            vDemoApp.OnWindowStateChanged(WindowState);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
            AdjustZetaAndItsCanvas();
        }

        private void AdjustZetaAndItsCanvas() {
            var adjuster = Container?.Resolve<ICanvasAndImageSizeAdjuster>();
            adjuster?.AdjustCanvasAndImage(ZetaCanvasContainer, ZetaCanvas, Zeta);
        }
    }
}
