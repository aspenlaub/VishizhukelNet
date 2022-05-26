using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Entities;
using VishizhukelEmptyApplication = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Application.Application;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication;

public class GuiToApplicationGate : GuiToApplicationGateBase<VishizhukelEmptyApplication, ApplicationModel> {
    public GuiToApplicationGate(IBusy busy, VishizhukelEmptyApplication application) : base(busy, application) {
    }
}