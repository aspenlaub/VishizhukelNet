using System;
using System.Linq;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.GUI;
using VishizhukelNetWebBrowserWindow = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.GUI.VishizhukelNetWebBrowserWindow;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App {
    public static bool IsIntegrationTest { get; private set; }
    private static bool OtherWindowsLaunched;

    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);
        IsIntegrationTest = e.Args.Any(a => a == "/UnitTest");
    }

    private void OnActivated(object sender, EventArgs e) {
        if (IsIntegrationTest || OtherWindowsLaunched) { return; }

        OtherWindowsLaunched = true;

        var emptyWindow = new VishizhukelNetEmptyWindow { NoTash = true };
        emptyWindow.Show();

        var browserWindow = new VishizhukelNetWebBrowserWindow { NoTash = true };
        browserWindow.Show();

        var webView2Window = new VishizhukelNetWebView2Window { NoTash = true };
        webView2Window.Show();
    }
}