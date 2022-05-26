﻿using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;

public class AlphaTextHandler : ISimpleTextHandler {
    private readonly IApplicationModel Model;
    private readonly IGuiAndAppHandler<ApplicationModel> GuiAndAppHandler;
    private readonly ISimpleSelectorHandler BetaSelectorHandler;
    private readonly ISimpleTextHandler DeltaTextHandler;

    public AlphaTextHandler(IApplicationModel model, IGuiAndAppHandler<ApplicationModel> guiAndAppHandler, ISimpleSelectorHandler betaSelectorHandler, ISimpleTextHandler deltaTextHandler) {
        Model = model;
        GuiAndAppHandler = guiAndAppHandler;
        BetaSelectorHandler = betaSelectorHandler;
        DeltaTextHandler = deltaTextHandler;
    }

    public async Task TextChangedAsync(string text) {
        if (Model.Alpha.Text == text) { return; }

        Model.Alpha.Text = text;
        Model.Alpha.Type = uint.TryParse(text, out _) ? StatusType.None : StatusType.Error;
        await BetaSelectorHandler.UpdateSelectableValuesAsync();
        await DeltaTextHandler.TextChangedAsync("");
        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }
}