using System;
using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IWebView : IWebBrowserOrView {
        IScriptStatement OnDocumentLoaded { get; set; }
        IScriptStatement ToExecute { get; set; }
        Func<string, Task> OnScriptCodeExecutedAsync { get; set; }
    }
}
