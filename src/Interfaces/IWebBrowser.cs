using MSHTML;
// ReSharper disable UnusedMemberInSuper.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IWebBrowser : IWebBrowserOrView {
    public IHTMLDocument3 Document { get; set; }
}