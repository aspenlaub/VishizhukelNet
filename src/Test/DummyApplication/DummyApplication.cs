using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DummyApplication {
    public class DummyApplication : ApplicationBase<DummyGuiAndApplicationSynchronizer, DummyApplicationModel> {
        public DummyApplication(IButtonNameToCommandMapper buttonNameToCommandMapper, DummyGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, DummyApplicationModel model) : base(buttonNameToCommandMapper, guiAndApplicationSynchronizer, model) { } protected override async Task EnableOrDisableButtonsAsync() {
            await Task.Delay(10);
        }

        public override void RegisterTypes() {
        }
    }
}
