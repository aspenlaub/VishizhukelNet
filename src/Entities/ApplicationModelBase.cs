using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities {
    // ReSharper disable once UnusedMember.Global
    public class ApplicationModelBase<TWebBrowserOrView> : IApplicationModelBase where TWebBrowserOrView : IWebBrowserOrView, new() {
        public bool IsBusy { get; set; }

        public ITextBox Status { get; set; } = new TextBox { Enabled = false };

        public WindowState WindowState { get; set; }

        public bool UsesRealBrowserOrView { get; set; }

        public IWebBrowserOrView WebBrowserOrView { get; } = new TWebBrowserOrView();

        public IWebBrowser WebBrowser => WebBrowserOrView as IWebBrowser;
        public IWebView WebView => WebBrowserOrView as IWebView;

        public ITextBox WebBrowserOrViewUrl { get; } = new TextBox();
        public ITextBox WebBrowserOrViewContentSource { get; } = new TextBox();
    }
}
