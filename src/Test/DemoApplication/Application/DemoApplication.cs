﻿using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Commands;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application {
    public class DemoApplication : ApplicationBase<IDemoGuiAndApplicationSynchronizer, DemoApplicationModel> {
        public IDemoHandlers Handlers { get; private set; }
        public IDemoCommands Commands { get; private set; }

        public DemoApplication(IButtonNameToCommandMapper buttonNameToCommandMapper, IDemoGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, DemoApplicationModel model) : base(buttonNameToCommandMapper, guiAndApplicationSynchronizer, model) { } protected override async Task EnableOrDisableButtonsAsync() {
            await Task.Delay(10);
        }

        public override void RegisterTypes() {
            var betaSelectorHandler = new BetaSelectorHandler(Model);
            Handlers = new DemoHandlers {
                BetaSelectorHandler = betaSelectorHandler
            };
            Commands = new DemoCommands {
                GammaCommand = new GammaCommand(Model)
            };
        }

        public override async Task OnLoadedAsync() {
            await Handlers.BetaSelectorHandler.UpdateSelectableBetaValuesAsync();
            await base.OnLoadedAsync();
        }

        public async Task AlphaTextChangedAsync(string text) {
            if (Model.Alpha.Text == text) { return; }

            Model.Alpha.Text = text;
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }
}
