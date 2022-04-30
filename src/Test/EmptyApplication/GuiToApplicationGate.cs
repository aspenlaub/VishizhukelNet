using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication;

public class GuiToApplicationGate : GuiToApplicationGateBase<Application.Application> {
    public GuiToApplicationGate(IBusy busy, Application.Application application) : base(busy, application) {
    }
}