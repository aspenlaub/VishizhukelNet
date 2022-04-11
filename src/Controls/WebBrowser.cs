using System;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using MSHTML;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls {
    public class WebBrowser : IWebBrowserWithDocument {
        public string Url { get; set; }
        public bool IsNavigating { get; set; }
        public IHTMLDocument3 Document { get; set; }
        public DateTime LastNavigationStartedAt { get; set; }
        public string AskedForNavigationToUrl { get; set; }

        public WebBrowser() {
            Url = "";
            IsNavigating = false;
            LastNavigationStartedAt = DateTime.MinValue;
            AskedForNavigationToUrl = "";
        }
    }
}
