﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Commands;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Application;

public class Application : ApplicationBase<IGuiAndApplicationSynchronizer, IApplicationModel> {
    public IApplicationHandlers Handlers { get; private set; }
    public IApplicationCommands Commands { get; private set; }

    public ITashHandler<IApplicationModel> TashHandler { get; private set; }
    private readonly ITashAccessor TashAccessor;
    private readonly ISimpleLogger SimpleLogger;
    private readonly ILogConfiguration LogConfiguration;
    private readonly IWebViewNavigationHelper WebViewNavigationHelper;

    public Application(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
        IGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, IApplicationModel model,
        ITashAccessor tashAccessor, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration, IApplicationLogger applicationLogger)
        : base(buttonNameToCommandMapper, toggleButtonNameToHandlerMapper, guiAndApplicationSynchronizer, model, applicationLogger) {
        TashAccessor = tashAccessor;
        SimpleLogger = simpleLogger;
        LogConfiguration = logConfiguration;
        WebViewNavigationHelper = new WebViewNavigationHelper<IApplicationModel>(model, applicationLogger, this, WebViewNavigatingHelper);
    }

    protected override async Task EnableOrDisableButtonsAsync() {
        Model.GoToUrl.Enabled = await Commands.GoToUrlCommand.ShouldBeEnabledAsync();
        Model.RunJs.Enabled = await Commands.RunJsCommand.ShouldBeEnabledAsync();
    }

    protected override void CreateCommandsAndHandlers() {
        Handlers = new ApplicationHandlers {
            WebViewUrlTextHandler = new WebViewUrlTextHandler(Model, this),
            WebViewContentSourceTextHandler = new WebViewContentSourceTextHandler(Model, this)
        };
        Commands = new ApplicationCommands {
            GoToUrlCommand = new GoToUrlCommand(Model, WebViewNavigationHelper),
            RunJsCommand = new RunJsCommand(Model, this)
        };
        var communicator = new TashCommunicatorBase<IApplicationModel>(TashAccessor, SimpleLogger, LogConfiguration);
        var selectors = new Dictionary<string, ISelector>();
        var selectorHandler = new TashSelectorHandler(Handlers, SimpleLogger, communicator, selectors);
        var verifyAndSetHandler = new TashVerifyAndSetHandler(Handlers, SimpleLogger, null, communicator, selectors);
        TashHandler = new TashHandler(TashAccessor, SimpleLogger, LogConfiguration, ButtonNameToCommandMapper, ToggleButtonNameToHandlerMapper, this, verifyAndSetHandler, selectorHandler, communicator);
    }

    public ITashTaskHandlingStatus<IApplicationModel> CreateTashTaskHandlingStatus() {
        return new TashTaskHandlingStatus<IApplicationModel>(Model, Process.GetCurrentProcess().Id);
    }
}