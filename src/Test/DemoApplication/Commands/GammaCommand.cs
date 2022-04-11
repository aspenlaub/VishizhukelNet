using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Commands {
    public class GammaCommand : ICommand {
        private readonly IApplicationModel Model;
        private readonly ISimpleTextHandler DeltaTextHandler;

        public GammaCommand(IApplicationModel model, ISimpleTextHandler deltaTextHandler) {
            Model = model;
            DeltaTextHandler = deltaTextHandler;
        }

        public async Task ExecuteAsync() {
            if (!Model.Gamma.Enabled) {
                return;
            }

            var alphaValue = uint.Parse(Model.Alpha.Text);
            var betaValue = uint.Parse(Model.Beta.SelectedItem.Name);
            var result = Model.MethodAdd.IsChecked ? alphaValue + betaValue : alphaValue * betaValue;
            await DeltaTextHandler.TextChangedAsync(result.ToString());
            Model.Status.Type = StatusType.Success;
            Model.Status.Text = Properties.Resources.CalculationSuccessful;
        }

        public async Task<bool> ShouldBeEnabledAsync() {
            var enabled = Model.Alpha.Type == StatusType.None && Model.Beta.SelectionMade;
            return await Task.FromResult(enabled);
        }
    }
}
