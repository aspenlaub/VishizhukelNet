using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DummyApplication {
    public class DummyGuiAndApplicationSynchronizer : GuiAndApplicationSynchronizerBase<DummyApplicationModel, DummyWindow> {
        public DummyGuiAndApplicationSynchronizer(DummyApplicationModel model, DummyWindow window) : base(model, window) {
        }

        public override void OnCursorChanged() {
        }
    }
}
