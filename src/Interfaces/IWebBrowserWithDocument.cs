using MSHTML;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IWebBrowserWithDocument : IWebBrowser {
        public IHTMLDocument3 Document { get; set; }
    }
}
