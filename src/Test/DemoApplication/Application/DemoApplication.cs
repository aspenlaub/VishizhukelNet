using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application {
    public class DemoApplication : ApplicationBase<IDemoGuiAndApplicationSynchronizer, DemoApplicationModel> {
        public DemoApplication(IButtonNameToCommandMapper buttonNameToCommandMapper, IDemoGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, DemoApplicationModel model) : base(buttonNameToCommandMapper, guiAndApplicationSynchronizer, model) { } protected override async Task EnableOrDisableButtonsAsync() {
            await Task.Delay(10);
        }

        public override void RegisterTypes() {
        }
    }
}
