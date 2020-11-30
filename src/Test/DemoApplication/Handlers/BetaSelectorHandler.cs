using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class BetaSelectorHandler : IBetaSelectorHandler {
        private readonly IDemoApplicationModel vModel;

        public BetaSelectorHandler(IDemoApplicationModel model) {
            vModel = model;
        }

        public async Task UpdateSelectableBetaValuesAsync() {
            await Task.Delay(10); // TODO: replace
        }

        public async Task SelectedBetaIndexChangedAsync(int selectedBetaIndex) {
            await Task.Delay(10); // TODO: replace
        }
    }
}
