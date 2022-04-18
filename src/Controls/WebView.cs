using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls {
    public class WebView : WebBrowserOrViewBase, IWebView {
        public string ScriptCodeToExecute { get; set; } = "";
        public Func<string, Task> OnScriptCodeExecutedAsync { get; set; } = _ => Task.CompletedTask;
    }
}
