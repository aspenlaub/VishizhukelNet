using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;

public class BetaSelectorHandler : ISimpleSelectorHandler {
    private readonly IApplicationModel _Model;
    private readonly IGuiAndAppHandler<ApplicationModel> _GuiAndAppHandler;
    private readonly DeltaTextHandler _DeltaTextHandler;

    public BetaSelectorHandler(IApplicationModel model, IGuiAndAppHandler<ApplicationModel> guiAndAppHandler, DeltaTextHandler deltaTextHandler) {
        _Model = model;
        _GuiAndAppHandler = guiAndAppHandler;
        _DeltaTextHandler = deltaTextHandler;
    }

    public async Task UpdateSelectableValuesAsync() {
        var choices = new List<uint>();
        if (uint.TryParse(_Model.Alpha.Text, out var alpha)) {
            choices.Add(alpha);
            choices.Add(alpha + 7);
            choices.Add(alpha + 24);
            choices.Add(alpha * 7);
            choices.Add(alpha * 24);
        }

        var selectables = choices.Distinct().OrderBy(x => x).Select(x => new Selectable { Guid = x.ToString(), Name = x.ToString() }).ToList();
        if (_Model.Beta.AreSelectablesIdentical(selectables)) { return; }

        _Model.Beta.UpdateSelectables(selectables);
        await _GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }

    public async Task SelectedIndexChangedAsync(int selectedIndex) {
        if (_Model.Beta.SelectedIndex == selectedIndex) { return; }

        _Model.Beta.SelectedIndex = selectedIndex;
        await _DeltaTextHandler.TextChangedAsync("");
        await _GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }
}