using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication {
    public class DemoGuiAndApplicationSynchronizer : GuiAndApplicationSynchronizerBase<IDemoApplicationModel, VishizhukelNetDemoWindow>, IDemoGuiAndApplicationSynchronizer {
        public DemoGuiAndApplicationSynchronizer(IDemoApplicationModel model, VishizhukelNetDemoWindow window) : base(model, window) {
        }
    }
}
