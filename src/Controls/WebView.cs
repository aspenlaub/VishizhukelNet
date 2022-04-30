using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;

public class WebView : WebBrowserOrViewBase, IWebView {
    public IScriptStatement OnDocumentLoaded { get; set; } = new ScriptStatement();
    public IScriptStatement ToExecute { get; set; } = new ScriptStatement();
    public bool IsWired { get; set; } = false;

    public Func<string, Task> OnScriptCodeExecutedAsync { get; set; } = _ => Task.CompletedTask;
}