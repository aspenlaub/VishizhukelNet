using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using MSHTML;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;

public class WebBrowser : WebBrowserOrViewBase, IWebBrowser {
    public IHTMLDocument3 Document { get; set; }

    public new void RevalidateDocument() {
        HasValidDocument = Document != null;
    }
}