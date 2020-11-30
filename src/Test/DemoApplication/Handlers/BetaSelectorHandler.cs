using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class BetaSelectorHandler : IBetaSelectorHandler {
        private readonly IDemoApplicationModel vModel;
        private readonly IGuiAndAppHandler vGuiAndAppHandler;

        public BetaSelectorHandler(IDemoApplicationModel model, IGuiAndAppHandler guiAndAppHandler) {
            vModel = model;
            vGuiAndAppHandler = guiAndAppHandler;
        }

        public async Task UpdateSelectableBetaValuesAsync() {
            var choices = new List<uint>();
            if (uint.TryParse(vModel.Alpha.Text, out var alpha)) {
                choices.Add(alpha);
                choices.Add(alpha + 7);
                choices.Add(alpha + 24);
                choices.Add(alpha * 7);
                choices.Add(alpha * 24);
            }

            var selectables = choices.Distinct().OrderBy(x => x).Select(x => new Selectable { Guid = x.ToString(), Name = x.ToString() }).ToList();
            if (vModel.Beta.AreSelectablesIdentical(selectables)) { return; }

            vModel.Beta.UpdateSelectables(selectables);
            await vGuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }

        public async Task SelectedBetaIndexChangedAsync(int selectedBetaIndex) {
            if (vModel.Beta.SelectedIndex == selectedBetaIndex) { return; }

            vModel.Beta.SelectedIndex = selectedBetaIndex;
            await vGuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }
}
