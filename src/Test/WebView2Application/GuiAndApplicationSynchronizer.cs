using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application;

public class GuiAndApplicationSynchronizer : GuiAndApplicationSynchronizerBase<IApplicationModel, VishizhukelNetWebView2Window>, IGuiAndApplicationSynchronizer {
    public GuiAndApplicationSynchronizer(IApplicationModel model, VishizhukelNetWebView2Window window, IApplicationLogger applicationLogger) : base(model, window, applicationLogger) {
    }
}