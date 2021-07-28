using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Commands;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application {
    public class DemoApplication : ApplicationBase<IDemoGuiAndApplicationSynchronizer, IDemoApplicationModel> {
        public IDemoHandlers Handlers { get; private set; }
        public IDemoCommands Commands { get; private set; }

        public ITashHandler<IDemoApplicationModel> TashHandler { get; private set; }
        private readonly ITashAccessor vTashAccessor;
        private readonly ISimpleLogger vSimpleLogger;
        private readonly ILogConfiguration vLogConfiguration;

        public DemoApplication(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
                IDemoGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, IDemoApplicationModel model,
                ITashAccessor tashAccessor, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration)
                : base(buttonNameToCommandMapper, toggleButtonNameToHandlerMapper, guiAndApplicationSynchronizer, model) {
            vTashAccessor = tashAccessor;
            vSimpleLogger = simpleLogger;
            vLogConfiguration = logConfiguration;
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
                ThetaHandler = new ThetaHandler(Model, this),
                MethodAddHandler = new MethodAddHandler(Model, deltaTextHandler),
                MethodMultiplyHandler = new MethodMultiplyHandler(Model, deltaTextHandler)
            };
            Commands = new DemoCommands {
                GammaCommand = new GammaCommand(Model, deltaTextHandler)
            };
            var communicator = new TashCommunicatorBase<IDemoApplicationModel>(vTashAccessor, vSimpleLogger, vLogConfiguration);
            var selectors = new Dictionary<string, ISelector> {
                { nameof(IDemoApplicationModel.Beta), Model.Beta }
            };
            var selectorHandler = new TashSelectorHandler(Handlers, vSimpleLogger, communicator, selectors);
            var verifyAndSetHandler = new TashVerifyAndSetHandler(Handlers, vSimpleLogger, null, communicator, selectors);
            TashHandler = new TashHandler(vTashAccessor, vSimpleLogger, vLogConfiguration, ButtonNameToCommandMapper, ToggleButtonNameToHandlerMapper, this, verifyAndSetHandler, selectorHandler, communicator);
        }

        public override async Task OnLoadedAsync() {
            await base.OnLoadedAsync();
            await Handlers.BetaSelectorHandler.UpdateSelectableValuesAsync();
            // TODO: remove
            await Handlers.ThetaHandler.CollectionChangedAsync(new List<ICollectionViewSourceEntity> {
                new DemoCollectionViewSourceEntity { Date = new DateTime(2021, 7, 30), Name = "Decreased", Balance = 2404.40 },
                new DemoCollectionViewSourceEntity { Date = new DateTime(2021, 7, 29), Name = "Increased", Balance = 2707.70 },
                new DemoCollectionViewSourceEntity { Date = new DateTime(2021, 7, 28), Name = "Unchanged", Balance = 2407.70 }
            });
        }

        public ITashTaskHandlingStatus<IDemoApplicationModel> CreateTashTaskHandlingStatus() {
            return new TashTaskHandlingStatus<IDemoApplicationModel>(Model, Process.GetCurrentProcess().Id);
        }
    }
}
