using System.Diagnostics;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Commands;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application {
    public class DemoApplication : ApplicationBase<IDemoGuiAndApplicationSynchronizer, DemoApplicationModel> {
        public IDemoHandlers Handlers { get; private set; }
        public IDemoCommands Commands { get; private set; }

        public ITashHandler<IDemoApplicationModel> TashHandler { get; private set; }
        private readonly ITashAccessor vTashAccessor;
        private readonly IApplicationLogger vApplicationLogger;

        public DemoApplication(IButtonNameToCommandMapper buttonNameToCommandMapper, IDemoGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, DemoApplicationModel model,
                ITashAccessor tashAccessor, IApplicationLogger applicationLogger)
                : base(buttonNameToCommandMapper, guiAndApplicationSynchronizer, model) {
            vTashAccessor = tashAccessor;
            vApplicationLogger = applicationLogger;
        }

        protected override async Task EnableOrDisableButtonsAsync() {
            Model.Gamma.Enabled = await Commands.GammaCommand.ShouldBeEnabledAsync();
        }

        protected override void CreateCommandsAndHandlers() {
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
            var communicator = new TashCommunicatorBase<IDemoApplicationModel>(vTashAccessor, vApplicationLogger);
            TashHandler = new TashHandler(vTashAccessor, vApplicationLogger, ButtonNameToCommandMapper, null, null, communicator);
        }

        public override async Task OnLoadedAsync() {
            await base.OnLoadedAsync();
            await Handlers.BetaSelectorHandler.UpdateSelectableValuesAsync();
        }

        public ITashTaskHandlingStatus<IDemoApplicationModel> CreateTashTaskHandlingStatus() {
            return new TashTaskHandlingStatus<IDemoApplicationModel>(Model, Process.GetCurrentProcess().Id);
        }
    }
}
