using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DummyApplication {
    public class DummyGuiAndApplicationSynchronizer
            : GuiAndApplicationSynchronizerBase<DummyApplicationModel, DummyWindow>,
            IGuiAndApplicationSynchronizer<DummyApplicationModel> {
        public DummyGuiAndApplicationSynchronizer(DummyApplicationModel model, DummyWindow window) : base(model, window) {
        }

        public override void OnCursorChanged() {
        }
    }
}
