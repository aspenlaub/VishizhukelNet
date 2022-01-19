using System.Linq;
using System.Windows;

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
    }
}
