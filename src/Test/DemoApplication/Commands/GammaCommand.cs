using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Commands {
    public class GammaCommand : ICommand {
        private readonly IDemoApplicationModel vModel;

        public GammaCommand(IDemoApplicationModel model) {
            vModel = model;
        }

        public async Task ExecuteAsync() {
            if (!vModel.Gamma.Enabled) {
                return;
            }

            await Task.Delay(10); // TODO: replace
        }

        public async Task<bool> ShouldBeEnabledAsync() {
            var enabled = vModel.Alpha.Type == StatusType.Success && vModel.Beta.SelectionMade;
             return await Task.FromResult(enabled);
        }
    }
}
