using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication;

public class GuiAndApplicationSynchronizer : GuiAndApplicationSynchronizerBase<ApplicationModel, VishizhukelNetDemoWindow> {
    public GuiAndApplicationSynchronizer(ApplicationModel model, VishizhukelNetDemoWindow window, IApplicationLogger applicationLogger) : base(model, window, applicationLogger) {
    }
}