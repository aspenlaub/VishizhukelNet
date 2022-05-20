using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Handlers;

public class ApplicationHandlers : IApplicationHandlers {
    public ISimpleTextHandler WebViewUrlTextHandler { get; set; }
    public ISimpleTextHandler WebViewContentSourceTextHandler { get; set; }
}