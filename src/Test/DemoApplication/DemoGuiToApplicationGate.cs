using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using VishizhukelDemoApplication = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application.DemoApplication;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication {
    public class DemoGuiToApplicationGate : GuiToApplicationGateBase<VishizhukelDemoApplication> {
        public DemoGuiToApplicationGate(IBusy busy, VishizhukelDemoApplication application) : base(busy, application) {
        }
    }
}
