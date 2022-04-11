using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers {
    public class WebBrowserNavigationHelper<TApplicationModel> : IWebBrowserNavigationHelper where TApplicationModel : IApplicationModelBase {
        private readonly TApplicationModel Model;
        private readonly IApplicationLogger ApplicationLogger;
        private readonly IGuiAndAppHandler GuiAndAppHandler;

        public int MaxSeconds => 600;

        private const int IntervalInMilliseconds = 500;

        public WebBrowserNavigationHelper(TApplicationModel model, IApplicationLogger applicationLogger, IGuiAndAppHandler guiAndAppHandler) {
            Model = model;
            ApplicationLogger = applicationLogger;
            GuiAndAppHandler = guiAndAppHandler;
        }

        public async Task<bool> NavigateToUrlAsync(string url) {
            ApplicationLogger.LogMessage($"App navigating to {url}");

            if (Model.UsesRealBrowser) {
                var attempts = MaxSeconds * 1000 / IntervalInMilliseconds;
                while (Model.WebBrowser.IsNavigating && attempts > 0) {
                    await Task.Delay(TimeSpan.FromMilliseconds(IntervalInMilliseconds));
                    attempts--;
                }

                if (Model.WebBrowser.IsNavigating) {
                    Model.Status.Text = string.Format(Properties.Resources.WebBrowserStillBusyAfter, MaxSeconds);
                    Model.Status.Type = StatusType.Error;
                    ApplicationLogger.LogMessage($"Problem when navigating to {url}");
                    return false;
                }
            }

            Model.WebBrowser.Url = "";
            await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();

            var minNavigationStartTime = DateTime.Now;

            Model.WebBrowser.Url = url;
            await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();

            if (Model.UsesRealBrowser) {
                var attempts = MaxSeconds * 1000 / IntervalInMilliseconds;
                while ((Model.WebBrowser.LastNavigationStartedAt < minNavigationStartTime || Model.WebBrowser.IsNavigating) && attempts > 0) {
                    await Task.Delay(TimeSpan.FromMilliseconds(IntervalInMilliseconds));
                    attempts--;
                }

                if (Model.WebBrowser.IsNavigating) {
                    ApplicationLogger.LogMessage("App failed");
                    Model.Status.Text = string.Format(Properties.Resources.RequestTimedOutAfter, MaxSeconds);
                    Model.Status.Type = StatusType.Error;
                    ApplicationLogger.LogMessage($"Problem when navigating to {url}");
                    return false;
                }
            }

            if (Model.WebBrowser.Document != null) {
                return true;
            }

            ApplicationLogger.LogMessage("App failed");
            Model.Status.Text = Properties.Resources.CouldNotLoadUrl;
            Model.Status.Type = StatusType.Error;
            return false;

        }
    }
}
