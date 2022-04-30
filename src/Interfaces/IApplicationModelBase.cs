using System.Windows;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IApplicationModelBase : IBusy {
    ITextBox Status { get; }

    WindowState WindowState { get; set; }

    bool UsesRealBrowserOrView { get; set; }
    IWebBrowserOrView WebBrowserOrView { get; }
    IWebBrowser WebBrowser { get; }
    IWebView WebView { get; }
    ITextBox WebBrowserOrViewUrl { get; }
    ITextBox WebBrowserOrViewContentSource { get; }
}