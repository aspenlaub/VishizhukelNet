using System;
using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IWebView : IWebBrowserOrView {
        string ScriptToExecuteOnDocumentLoaded { get; set; }
        string ScriptCodeToExecute { get; set; }
        Func<string, Task> OnScriptCodeExecutedAsync { get; set; }
    }
}
