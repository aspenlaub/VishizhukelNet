using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities {
    public class WebViewApplicationModelBase : ApplicationModelBase, IWebViewApplicationModelBase {
        public IWebView WebView { get; } = new WebView();

        public ITextBox WebViewUrl { get; } = new TextBox();
        public ITextBox WebViewContentSource { get; } = new TextBox();
    }
}
