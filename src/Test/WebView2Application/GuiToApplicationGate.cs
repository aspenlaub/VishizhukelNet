using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Entities;
using VishizhukelWebView2Application = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Application.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application;

public class GuiToApplicationGate : GuiToWebViewApplicationGateBase<VishizhukelWebView2Application, ApplicationModel> {
    public GuiToApplicationGate(IBusy busy, VishizhukelWebView2Application application) : base(busy, application) {
    }
}