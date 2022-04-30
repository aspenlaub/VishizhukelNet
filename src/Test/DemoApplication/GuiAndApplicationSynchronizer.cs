using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication;

public class GuiAndApplicationSynchronizer : GuiAndApplicationSynchronizerBase<IApplicationModel, VishizhukelNetDemoWindow>, IGuiAndApplicationSynchronizer {
    public GuiAndApplicationSynchronizer(IApplicationModel model, VishizhukelNetDemoWindow window, IApplicationLogger applicationLogger) : base(model, window, applicationLogger) {
    }
}