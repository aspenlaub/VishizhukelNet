using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Commands;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Application;

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
        await Task.CompletedTask;
    }

    protected override void CreateCommandsAndHandlers() {
        Handlers = new ApplicationHandlers();
        Commands = new ApplicationCommands();
        var communicator = new TashCommunicatorBase<IApplicationModel>(_TashAccessor, SimpleLogger, _MethodNamesFromStackFramesExtractor);
        var selectors = new Dictionary<string, ISelector>();
        var selectorHandler = new TashSelectorHandler(Handlers, SimpleLogger, communicator, selectors, _MethodNamesFromStackFramesExtractor);
        var verifyAndSetHandler = new TashVerifyAndSetHandler(Handlers, SimpleLogger, selectorHandler, communicator, selectors, _MethodNamesFromStackFramesExtractor);
        TashHandler = new TashHandler(_TashAccessor, SimpleLogger, ButtonNameToCommandMapper, ToggleButtonNameToHandlerMapper, this, verifyAndSetHandler, selectorHandler, communicator, _MethodNamesFromStackFramesExtractor);
    }

    public ITashTaskHandlingStatus<ApplicationModel> CreateTashTaskHandlingStatus() {
        return new TashTaskHandlingStatus<ApplicationModel>(Model, Process.GetCurrentProcess().Id);
    }
}