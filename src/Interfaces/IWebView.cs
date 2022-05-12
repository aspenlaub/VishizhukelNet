namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IWebView : IWebBrowserOrView {
    IScriptStatement OnDocumentLoaded { get; set; }
    bool IsWired { get; set; }
}