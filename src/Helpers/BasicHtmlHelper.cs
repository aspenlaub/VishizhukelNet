using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using MSHTML;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers {
    public class BasicHtmlHelper : IBasicHtmlHelper {
        public string DocumentToHtml(IHTMLDocument3 document) {
            return document.documentElement.innerHTML;
        }

        public IHTMLDocument3 ObjectAsDocument(object document) {
            return document as IHTMLDocument3;
        }
    }
}
