using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication {
    public class DemoGuiAndApplicationSynchronizer : GuiAndApplicationSynchronizerBase<DemoApplicationModel, DemoWindow>, IDemoGuiAndApplicationSynchronizer {
        public DemoGuiAndApplicationSynchronizer(DemoApplicationModel model, DemoWindow window) : base(model, window) {
        }
    }
}
