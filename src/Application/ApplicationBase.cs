using System;
using System.Threading.Tasks;
using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using MSHTML;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Application {
    public abstract class ApplicationBase<TGuiAndApplicationSynchronizer, TModel> : IGuiAndAppHandler
        where TModel : class, IApplicationModelBase
        where TGuiAndApplicationSynchronizer : IGuiAndApplicationSynchronizer<TModel> {
        protected readonly IButtonNameToCommandMapper ButtonNameToCommandMapper;
        protected readonly IToggleButtonNameToHandlerMapper ToggleButtonNameToHandlerMapper;
        protected readonly TGuiAndApplicationSynchronizer GuiAndApplicationSynchronizer;
        protected readonly TModel Model;
        protected readonly IBasicHtmlHelper BasicHtmlHelper;
        protected readonly IApplicationLogger ApplicationLogger;

        protected ApplicationBase(IButtonNameToCommandMapper buttonNameToCommandMapper, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper,
                TGuiAndApplicationSynchronizer guiAndApplicationSynchronizer, TModel model, IBasicHtmlHelper basicHtmlHelper, IApplicationLogger applicationLogger) {
            ButtonNameToCommandMapper = buttonNameToCommandMapper;
            ToggleButtonNameToHandlerMapper = toggleButtonNameToHandlerMapper;
            GuiAndApplicationSynchronizer = guiAndApplicationSynchronizer;
            Model = model;
            BasicHtmlHelper = basicHtmlHelper;
            ApplicationLogger = applicationLogger;
        }

        protected abstract Task EnableOrDisableButtonsAsync();
        protected abstract void CreateCommandsAndHandlers();

        public virtual async Task OnLoadedAsync() {
            CreateCommandsAndHandlers();
            Model.WebBrowserOrViewUrl.Text = "http://localhost/";
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }

        public async Task EnableOrDisableButtonsThenSyncGuiAndAppAsync() {
            await EnableOrDisableButtonsAsync();
            await GuiAndApplicationSynchronizer.OnModelDataChangedAsync();
        }

        public void IndicateBusy(bool force) {
            GuiAndApplicationSynchronizer.IndicateBusy(force);
        }

        public void OnWindowStateChanged(WindowState windowState) {
            Model.WindowState = windowState;
        }

        public async Task OnWebBrowserNavigatingAsync(Uri uri) {
            Model.WebBrowser.IsNavigating = uri != null;
            Model.WebBrowserOrViewUrl.Text = uri == null ? "(off road)" : uri.OriginalString;
            Model.WebBrowser.Document = null;
            Model.WebBrowser.LastNavigationStartedAt = DateTime.Now;
            Model.WebBrowserOrViewContentSource.Text = "";
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
            ApplicationLogger.LogMessage($"GUI navigating to {Model.WebBrowserOrViewUrl.Text}");
            IndicateBusy(true);
        }

        public async Task OnWebBrowserLoadCompletedAsync(object documentObject) {
            var document = BasicHtmlHelper.ObjectAsDocument(documentObject);
            await OnWebBrowserLoadCompletedAsync(document, BasicHtmlHelper.DocumentToHtml(document));
        }

        public async Task OnWebBrowserLoadCompletedAsync(IHTMLDocument3 document, string documentAsString) {
            ApplicationLogger.LogMessage($"GUI navigation complete: {Model.WebBrowserOrViewUrl.Text}");
            Model.WebBrowser.Document = document;
            Model.WebBrowser.IsNavigating = false;
            Model.WebBrowser.HasValidDocument = document != null;
            GuiAndApplicationSynchronizer.OnWebBrowserLoadCompleted();
            Model.WebBrowserOrViewContentSource.Text = documentAsString;
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
            IndicateBusy(true);
        }

        public async Task OnWebViewNavigationStartingAsync(string uri) {
            Model.WebView.IsNavigating = uri != null;
            Model.WebBrowserOrViewUrl.Text = uri ?? "(off road)";
            Model.WebView.LastNavigationStartedAt = DateTime.Now;
            Model.WebBrowserOrViewContentSource.Text = "";
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
            ApplicationLogger.LogMessage($"GUI navigating to {Model.WebBrowserOrViewUrl.Text}");
            IndicateBusy(true);
        }

        public async Task OnWebViewNavigationCompletedAsync(string contentSource, bool isSuccess) {
            ApplicationLogger.LogMessage($"GUI navigation complete: {Model.WebBrowserOrViewUrl.Text}");
            Model.WebView.IsNavigating = false;
            Model.WebBrowserOrViewContentSource.Text = contentSource;
            Model.WebView.HasValidDocument = isSuccess;
            await EnableOrDisableButtonsThenSyncGuiAndAppAsync();
            IndicateBusy(true);
        }
    }
}
