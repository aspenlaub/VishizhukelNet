namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IWebViewApplicationModelBase : IApplicationModelBase {
        IWebView WebView { get; }
        ITextBox WebViewUrl { get; }
        ITextBox WebViewContentSource { get; }
    }
}
