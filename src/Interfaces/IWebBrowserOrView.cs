using System;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IWebBrowserOrView {
        string Url { get; set; }
        bool IsNavigating { get; set; }
        DateTime LastNavigationStartedAt { get; set; }
        string AskedForNavigationToUrl { get; set; }
        bool HasValidDocument { get; set; }
    }
}
