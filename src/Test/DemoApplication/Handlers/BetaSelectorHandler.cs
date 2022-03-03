using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class BetaSelectorHandler : ISimpleSelectorHandler {
        private readonly IDemoApplicationModel Model;
        private readonly IGuiAndAppHandler GuiAndAppHandler;
        private readonly DeltaTextHandler DeltaTextHandler;

        public BetaSelectorHandler(IDemoApplicationModel model, IGuiAndAppHandler guiAndAppHandler, DeltaTextHandler deltaTextHandler) {
            Model = model;
            GuiAndAppHandler = guiAndAppHandler;
            DeltaTextHandler = deltaTextHandler;
        }

        public async Task UpdateSelectableValuesAsync() {
            var choices = new List<uint>();
            if (uint.TryParse(Model.Alpha.Text, out var alpha)) {
                choices.Add(alpha);
                choices.Add(alpha + 7);
                choices.Add(alpha + 24);
                choices.Add(alpha * 7);
                choices.Add(alpha * 24);
            }

            var selectables = choices.Distinct().OrderBy(x => x).Select(x => new Selectable { Guid = x.ToString(), Name = x.ToString() }).ToList();
            if (Model.Beta.AreSelectablesIdentical(selectables)) { return; }

            Model.Beta.UpdateSelectables(selectables);
            await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }

        public async Task SelectedIndexChangedAsync(int selectedIndex) {
            if (Model.Beta.SelectedIndex == selectedIndex) { return; }

            Model.Beta.SelectedIndex = selectedIndex;
            await DeltaTextHandler.TextChangedAsync("");
            await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }
}
