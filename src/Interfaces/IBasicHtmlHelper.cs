using MSHTML;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IBasicHtmlHelper {
        IHTMLDocument3 ObjectAsDocument(object document);
        string DocumentToHtml(IHTMLDocument3 document);
    }
}
