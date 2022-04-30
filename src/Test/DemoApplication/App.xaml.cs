using System.Linq;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.GUI;
using VishizhukelNetWebBrowserWindow = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.GUI.VishizhukelNetWebBrowserWindow;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App {
    public static bool IsIntegrationTest { get; private set; }

    private void LaunchWindows(object sender, StartupEventArgs e) {
        IsIntegrationTest = e.Args.Any(a => a == "/UnitTest");
        var windowUnderTestClassName = nameof(VishizhukelNetDemoWindow);
        if (IsIntegrationTest) {
            const string tag = "/Window=";
            var arg = e.Args.Single(a => a.StartsWith(tag));
            windowUnderTestClassName = arg.Substring(tag.Length);
        }

        LaunchWindowIfNeeded<VishizhukelNetDemoWindow>(windowUnderTestClassName);
        LaunchWindowIfNeeded<VishizhukelNetEmptyWindow>(windowUnderTestClassName);
        LaunchWindowIfNeeded<VishizhukelNetWebBrowserWindow>(windowUnderTestClassName);
        LaunchWindowIfNeeded<VishizhukelNetWebView2Window>(windowUnderTestClassName);
    }

    private void LaunchWindowIfNeeded<TWindow>(string windowUnderTestClassName) where TWindow : Window, IVishizhukelNetWindowUnderTest, new() {
        var windowClassName = typeof(TWindow).Name;
        if (IsIntegrationTest && windowUnderTestClassName != windowClassName) {
            return;
        }

        var window = new TWindow { IsWindowUnderTest = windowUnderTestClassName == windowClassName };
        window.Show();
    }
}