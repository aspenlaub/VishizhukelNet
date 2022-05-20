using System;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;

public class WebView : IWebView {
    public string Url { get; set; }
    public bool IsNavigating { get; set; }
    public DateTime LastNavigationStartedAt { get; set; }
    public string LastUrl { get; set; }
    public bool HasValidDocument { get; set; }
    public IScriptStatement OnDocumentLoaded { get; set; }
    public bool IsWired { get; set; }

    public WebView() {
        Url = Urls.None;
        IsNavigating = false;
        LastNavigationStartedAt = DateTime.MinValue;
        LastUrl = "";
        HasValidDocument = false;
        OnDocumentLoaded = new ScriptStatement();
        IsWired = false;
    }
}