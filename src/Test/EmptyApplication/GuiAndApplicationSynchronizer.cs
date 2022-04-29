using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication {
    public class GuiAndApplicationSynchronizer : GuiAndApplicationSynchronizerBase<IApplicationModel, VishizhukelNetEmptyWindow>, IGuiAndApplicationSynchronizer {
        public GuiAndApplicationSynchronizer(IApplicationModel model, VishizhukelNetEmptyWindow window, IApplicationLogger applicationLogger) : base(model, window, applicationLogger) {
        }
    }
}
