using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Commands {
    public class GammaCommand : ICommand {
        private readonly IDemoApplicationModel vModel;
        private readonly IDeltaTextChanged vDeltaTextChanged;

        public GammaCommand(IDemoApplicationModel model, IDeltaTextChanged deltaTextChanged) {
            vModel = model;
            vDeltaTextChanged = deltaTextChanged;
        }

        public async Task ExecuteAsync() {
            if (!vModel.Gamma.Enabled) {
                return;
            }

            await vDeltaTextChanged.DeltaTextChangedAsync((uint.Parse(vModel.Alpha.Text) + uint.Parse(vModel.Beta.SelectedItem.Name)).ToString());
        }

        public async Task<bool> ShouldBeEnabledAsync() {
            var enabled = vModel.Alpha.Type == StatusType.None && vModel.Beta.SelectionMade;
             return await Task.FromResult(enabled);
        }
    }
}
