using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Commands;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Handlers;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces;
using MSHTML;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Application {
    public class Application : ApplicationBase<IGuiAndApplicationSynchronizer, IApplicationModel> {
        public IApplicationHandlers Handlers { get; private set; }
        public IApplicationCommands Commands { get; private set; }

        public ITashHandler<IApplicationModel> TashHandler { get; private set; }
        private readonly ITashAccessor TashAccessor;
        private readonly ISimpleLogger SimpleLogger;
        private readonly ILogConfiguration LogConfiguration;
        private readonly IApplicationLogger ApplicationLogger;
        private readonly IBasicHtmlHelper BasicHtmlHelper;

        public Application(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
                IGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, IApplicationModel model,
                ITashAccessor tashAccessor, ISimpleLogger simpleLogger, ILogConfiguration logConfiguration, IApplicationLogger applicationLogger,
                IBasicHtmlHelper basicHtmlHelper)
                : base(buttonNameToCommandMapper, toggleButtonNameToHandlerMapper, guiAndApplicationSynchronizer, model) {
            TashAccessor = tashAccessor;
            SimpleLogger = simpleLogger;
            LogConfiguration = logConfiguration;
            ApplicationLogger = applicationLogger;
            BasicHtmlHelper = basicHtmlHelper;
        }

        protected override async Task EnableOrDisableButtonsAsync() {
            await Task.CompletedTask;
        }

        protected override void CreateCommandsAndHandlers() {
            Handlers = new ApplicationHandlers();
            Commands = new ApplicationCommands();
            var communicator = new TashCommunicatorBase<IApplicationModel>(TashAccessor, SimpleLogger, LogConfiguration);
            var selectors = new Dictionary<string, ISelector>();
            var selectorHandler = new TashSelectorHandler(Handlers, SimpleLogger, communicator, selectors);
            var verifyAndSetHandler = new TashVerifyAndSetHandler(Handlers, SimpleLogger, null, communicator, selectors);
            TashHandler = new TashHandler(TashAccessor, SimpleLogger, LogConfiguration, ButtonNameToCommandMapper, ToggleButtonNameToHandlerMapper, this, verifyAndSetHandler, selectorHandler, communicator);
        }

        public ITashTaskHandlingStatus<IApplicationModel> CreateTashTaskHandlingStatus() {
            return new TashTaskHandlingStatus<IApplicationModel>(Model, Process.GetCurrentProcess().Id);
        }

        public void OnWebBrowserNavigating(Uri uri) {
            Model.WebBrowser.IsNavigating = uri != null;
            Model.WebBrowserUrl.Text = uri == null ? "(off road)" : uri.OriginalString;
            Model.WebBrowser.Document = null;
            Model.WebBrowser.LastNavigationStartedAt = DateTime.Now;
            ApplicationLogger.LogMessage($"GUI navigating to {Model.WebBrowserUrl.Text}");
            IndicateBusy(true);
        }

        public void OnWebBrowserLoadCompleted(object documentObject) {
            var document = BasicHtmlHelper.ObjectAsDocument(documentObject);
            OnWebBrowserLoadCompleted(document, BasicHtmlHelper.DocumentToHtml(document));
        }

        public void OnWebBrowserLoadCompleted(IHTMLDocument3 document, string documentAsString) {
            ApplicationLogger.LogMessage($"GUI navigation complete: {Model.WebBrowserUrl.Text}");
            Model.WebBrowser.Document = document;
            Model.WebBrowser.IsNavigating = false;
            GuiAndApplicationSynchronizer.OnWebBrowserLoadCompleted();
            IndicateBusy(true);
        }

    }
}
