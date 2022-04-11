using System;
using System.Linq;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.GUI;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication {
    /// <summary>
    /// Interaction logic for DemoApp.xaml
    /// </summary>
    public partial class DemoApp {
        public static bool IsIntegrationTest { get; private set; }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            IsIntegrationTest = e.Args.Any(a => a == "/UnitTest");
        }

        private void OnActivated(object sender, EventArgs e) {
            if (IsIntegrationTest) { return; }

            var emptyWindow = new VishizhukelNetEmptyWindow();
            emptyWindow.Show();
            var browserWindow = new VishizhukelNetWebBrowserWindow();
            browserWindow.Show();
        }
    }
}
