using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Commands;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application {
    public class DemoApplication : ApplicationBase<IDemoGuiAndApplicationSynchronizer, DemoApplicationModel>, IDeltaTextChanged {
        public IDemoHandlers Handlers { get; private set; }
        public IDemoCommands Commands { get; private set; }

        public DemoApplication(IButtonNameToCommandMapper buttonNameToCommandMapper, IDemoGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, DemoApplicationModel model) : base(
            buttonNameToCommandMapper, guiAndApplicationSynchronizer, model) {

        }

        protected override async Task EnableOrDisableButtonsAsync() {
            Model.Gamma.Enabled = await Commands.GammaCommand.ShouldBeEnabledAsync();
        }

        public override void RegisterTypes() {
            var betaSelectorHandler = new BetaSelectorHandler(Model, this);
            Handlers = new DemoHandlers {
                BetaSelectorHandler = betaSelectorHandler
            };
            Commands = new DemoCommands {
                GammaCommand = new GammaCommand(Model, this)
            };
        }

        public override async Task OnLoadedAsync() {
            await Handlers.BetaSelectorHandler.UpdateSelectableBetaValuesAsync();
            await base.OnLoadedAsync();
        }

        public async Task AlphaTextChangedAsync(string text) {
            if (Model.Alpha.Text == text) { return; }

            Model.Alpha.Text = text;
            Model.Alpha.Type = uint.TryParse(text, out _) ? StatusType.None : StatusType.Error;
            await Handlers.BetaSelectorHandler.UpdateSelectableBetaValuesAsync();
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }

        public async Task DeltaTextChangedAsync(string text) {
            if (Model.Delta.Text == text) { return; }

            Model.Delta.Text = text;
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }
}
