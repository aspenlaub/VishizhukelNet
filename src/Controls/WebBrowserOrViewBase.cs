using System;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;

public class WebBrowserOrViewBase : IWebBrowserOrView {
    public string Url { get; set; }
    public bool IsNavigating { get; set; }
    public DateTime LastNavigationStartedAt { get; set; }
    public string LastUrl { get; set; }
    public bool HasValidDocument { get; set; }

    public WebBrowserOrViewBase() {
        Url = "";
        IsNavigating = false;
        LastNavigationStartedAt = DateTime.MinValue;
        LastUrl = "";
        HasValidDocument = false;
    }

    public void RevalidateDocument() {
    }
}