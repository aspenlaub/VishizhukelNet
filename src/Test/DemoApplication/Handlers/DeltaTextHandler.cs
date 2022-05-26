﻿using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;

public class DeltaTextHandler : ISimpleTextHandler {
    private readonly IApplicationModel Model;
    private readonly IGuiAndAppHandler<ApplicationModel> GuiAndAppHandler;

    public DeltaTextHandler(IApplicationModel model, IGuiAndAppHandler<ApplicationModel> guiAndAppHandler) {
        Model = model;
        GuiAndAppHandler = guiAndAppHandler;
    }

    public async Task TextChangedAsync(string text) {
        if (Model.Delta.Text == text) { return; }

        Model.Delta.Text = text;
        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }
}