using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class BetaSelectorHandler : ISimpleSelectorHandler {
        private readonly IDemoApplicationModel vModel;
        private readonly IGuiAndAppHandler vGuiAndAppHandler;
        private readonly DeltaTextHandler vDeltaTextHandler;

        public BetaSelectorHandler(IDemoApplicationModel model, IGuiAndAppHandler guiAndAppHandler, DeltaTextHandler deltaTextHandler) {
            vModel = model;
            vGuiAndAppHandler = guiAndAppHandler;
            vDeltaTextHandler = deltaTextHandler;
        }

        public async Task UpdateSelectableValuesAsync() {
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

        public async Task SelectedIndexChangedAsync(int selectedIndex) {
            if (vModel.Beta.SelectedIndex == selectedIndex) { return; }

            vModel.Beta.SelectedIndex = selectedIndex;
            await vDeltaTextHandler.TextChangedAsync("");
            await vGuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }
}
