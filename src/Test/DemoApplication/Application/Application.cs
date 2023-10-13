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

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;

public class Application : ApplicationBase<IGuiAndApplicationSynchronizer<ApplicationModel>, ApplicationModel> {
    public IApplicationHandlers Handlers { get; private set; }
    public IApplicationCommands Commands { get; private set; }

    public ITashHandler<ApplicationModel> TashHandler { get; private set; }
    private readonly ITashAccessor _TashAccessor;
    private readonly IMethodNamesFromStackFramesExtractor _MethodNamesFromStackFramesExtractor;

    public Application(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
            IGuiAndApplicationSynchronizer<ApplicationModel> guiAndApplicationSynchronizer, ApplicationModel model, ITashAccessor tashAccessor,
            ISimpleLogger simpleLogger, IMethodNamesFromStackFramesExtractor methodNamesFromStackFramesExtractor)
        : base(buttonNameToCommandMapper, toggleButtonNameToHandlerMapper, guiAndApplicationSynchronizer, model, simpleLogger) {
        _TashAccessor = tashAccessor;
        _MethodNamesFromStackFramesExtractor = methodNamesFromStackFramesExtractor;
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
        var communicator = new TashCommunicatorBase<IApplicationModel>(_TashAccessor, SimpleLogger, _MethodNamesFromStackFramesExtractor);
        var selectors = new Dictionary<string, ISelector> {
            { nameof(IApplicationModel.Beta), Model.Beta }
        };
        var selectorHandler = new TashSelectorHandler(Handlers, SimpleLogger, communicator, selectors, _MethodNamesFromStackFramesExtractor);
        var verifyAndSetHandler = new TashVerifyAndSetHandler(Handlers, SimpleLogger, selectorHandler, communicator, selectors, _MethodNamesFromStackFramesExtractor);
        TashHandler = new TashHandler(_TashAccessor, SimpleLogger, ButtonNameToCommandMapper, ToggleButtonNameToHandlerMapper, this, verifyAndSetHandler, selectorHandler, communicator, _MethodNamesFromStackFramesExtractor);
    }

    public override async Task OnLoadedAsync() {
        await base.OnLoadedAsync();
        if (!App.IsIntegrationTest) {
            var items = new List<DemoCollectionViewSourceEntity> {
                new DemoCollectionViewSourceEntity {
                    Date = new DateTime(2022, 1, 19),
                    Name = "Some name",
                    Balance = 2470.70
                }

            };
            await Handlers.ThetaHandler.CollectionChangedAsync(items);
        }
        await Handlers.BetaSelectorHandler.UpdateSelectableValuesAsync();
    }

    public ITashTaskHandlingStatus<ApplicationModel> CreateTashTaskHandlingStatus() {
        return new TashTaskHandlingStatus<ApplicationModel>(Model, Process.GetCurrentProcess().Id);
    }
}