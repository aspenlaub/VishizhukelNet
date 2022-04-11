using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication {
    public class GuiAndApplicationSynchronizer : GuiAndApplicationSynchronizerBase<IApplicationModel, VishizhukelNetEmptyWindow>, IGuiAndApplicationSynchronizer {
        public GuiAndApplicationSynchronizer(IApplicationModel model, VishizhukelNetEmptyWindow window) : base(model, window) {
        }
    }
}
