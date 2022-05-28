using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;

public class Application : ApplicationBase<IGuiAndApplicationSynchronizer<ApplicationModel>, ApplicationModel> {
    public IApplicationHandlers Handlers { get; private set; }
    public IApplicationCommands Commands { get; private set; }

    public ITashHandler<ApplicationModel> TashHandler { get; private set; }
    private readonly ITashAccessor TashAccessor;
    private readonly ILogConfigurationFactory LogConfigurationFactory;

    public Application(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
        IGuiAndApplicationSynchronizer<ApplicationModel> guiAndApplicationSynchronizer, ApplicationModel model, ITashAccessor tashAccessor,
        ISimpleLogger simpleLogger, ILogConfigurationFactory logConfigurationFactory)
        : base(buttonNameToCommandMapper, toggleButtonNameToHandlerMapper, guiAndApplicationSynchronizer, model, simpleLogger) {
        TashAccessor = tashAccessor;
        LogConfigurationFactory = logConfigurationFactory;
    }

    protected override async Task EnableOrDisableButtonsAsync() {
        Model.Gamma.Enabled = await Commands.GammaCommand.ShouldBeEnabledAsync();
        Model.Iota.Enabled = await Commands.IotaCommand.ShouldBeEnabledAsync();
        Model.Kappa.Enabled = await Commands.KappaCommand.ShouldBeEnabledAsync();
    }

    protected override void CreateCommandsAndHandlers() {
        var deltaTextHandler = new DeltaTextHandler(Model, this);
        var betaSelectorHandler = new BetaSelectorHandler(Model, this, deltaTextHandler);
        var alphaTextHandler = new AlphaTextHandler(Model, this, betaSelectorHandler, deltaTextHandler);
        Handlers = new ApplicationHandlers {
            AlphaTextHandler = alphaTextHandler,
            BetaSelectorHandler = betaSelectorHandler,
            DeltaTextHandler = deltaTextHandler,
            ThetaHandler = new ThetaHandler<ApplicationModel>(Model, this),
            MethodAddHandler = new MethodAddHandler(Model, deltaTextHandler),
            MethodMultiplyHandler = new MethodMultiplyHandler(Model, deltaTextHandler)
        };
        Commands = new ApplicationCommands {
            GammaCommand = new GammaCommand(Model, deltaTextHandler),
            IotaCommand = new IotaCommand(Model),
            KappaCommand = new KappaCommand(Model)
        };
        var communicator = new TashCommunicatorBase<IApplicationModel>(TashAccessor, SimpleLogger, LogConfigurationFactory);
        var selectors = new Dictionary<string, ISelector> {
            { nameof(IApplicationModel.Beta), Model.Beta }
        };
        var selectorHandler = new TashSelectorHandler(Handlers, SimpleLogger, communicator, selectors);
        var verifyAndSetHandler = new TashVerifyAndSetHandler(Handlers, SimpleLogger, selectorHandler, communicator, selectors);
        TashHandler = new TashHandler(TashAccessor, SimpleLogger, LogConfigurationFactory, ButtonNameToCommandMapper, ToggleButtonNameToHandlerMapper, this, verifyAndSetHandler, selectorHandler, communicator);
    }

    public override async Task OnLoadedAsync() {
        await base.OnLoadedAsync();
        if (!App.IsIntegrationTest) {
            var items = new List<IDemoCollectionViewSourceEntity> {
                new DemoCollectionViewSourceEntity {
                    Date = new DateTime(2022, 1, 19),
                    Name = "Some name",
                    Balance = 2470.70
                }

            }.Cast<ICollectionViewSourceEntity>().ToList();
            await Handlers.ThetaHandler.CollectionChangedAsync(items);
        }
        await Handlers.BetaSelectorHandler.UpdateSelectableValuesAsync();
    }

    public ITashTaskHandlingStatus<ApplicationModel> CreateTashTaskHandlingStatus() {
        return new TashTaskHandlingStatus<ApplicationModel>(Model, Process.GetCurrentProcess().Id);
    }
}