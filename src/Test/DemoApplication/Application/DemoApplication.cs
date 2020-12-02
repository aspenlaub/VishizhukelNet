using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Commands;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application {
    public class DemoApplication : ApplicationBase<IDemoGuiAndApplicationSynchronizer, DemoApplicationModel> {
        public IDemoHandlers Handlers { get; private set; }
        public IDemoCommands Commands { get; private set; }

        public DemoApplication(IButtonNameToCommandMapper buttonNameToCommandMapper, IDemoGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, DemoApplicationModel model) : base(
            buttonNameToCommandMapper, guiAndApplicationSynchronizer, model) {
        }

        protected override async Task EnableOrDisableButtonsAsync() {
            Model.Gamma.Enabled = await Commands.GammaCommand.ShouldBeEnabledAsync();
        }

        public override void RegisterTypes() {
            var deltaTextHandler = new DeltaTextHandler(Model, this);
            var betaSelectorHandler = new BetaSelectorHandler(Model, this, deltaTextHandler);
            var alphaTextHandler = new AlphaTextHandler(Model, this, betaSelectorHandler, deltaTextHandler);
            Handlers = new DemoHandlers {
                AlphaTextHandler = alphaTextHandler,
                BetaSelectorHandler = betaSelectorHandler,
                DeltaTextHandler = deltaTextHandler,
                MethodAddHandler = new MethodAddHandler(Model, deltaTextHandler),
                MethodMultiplyHandler = new MethodMultiplyHandler(Model, deltaTextHandler)
            };
            Commands = new DemoCommands {
                GammaCommand = new GammaCommand(Model, deltaTextHandler)
            };
        }

        public override async Task OnLoadedAsync() {
            await Handlers.BetaSelectorHandler.UpdateSelectableValuesAsync();
            await base.OnLoadedAsync();
        }
    }
}
