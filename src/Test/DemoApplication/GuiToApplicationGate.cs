using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using VishizhukelDemoApplication = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication;

public class GuiToApplicationGate : GuiToApplicationGateBase<VishizhukelDemoApplication, ApplicationModel> {
    public GuiToApplicationGate(IBusy busy, VishizhukelDemoApplication application) : base(busy, application) {
    }
}