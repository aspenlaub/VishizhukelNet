using System;
using MSHTML;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IWebBrowser {
        string Url { get; set; }
        bool IsNavigating { get; set; }
        IHTMLDocument3 Document { get; set; }
        DateTime LastNavigationStartedAt { get; set; }
        string AskedForNavigationToUrl { get; set; }
    }
}
