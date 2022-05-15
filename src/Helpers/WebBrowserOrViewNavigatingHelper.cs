using System;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;

public class WebBrowserOrViewNavigatingHelper : IWebBrowserOrViewNavigatingHelper {
    public const int QuickSeconds = 5, MaxSeconds = 600;
    private const int IntervalInMilliseconds = 500, LargeIntervalInMilliseconds = 5000;
    private const int DoubleCheckIntervalInMilliseconds = 200, DoubleCheckLargeIntervalInMilliseconds = 1000;

    private readonly IApplicationModelBase Model;
    private readonly IApplicationLogger ApplicationLogger;

    public WebBrowserOrViewNavigatingHelper(IApplicationModelBase model, IApplicationLogger applicationLogger) {
        Model = model;
        ApplicationLogger = applicationLogger;
    }

    public async Task<bool> WaitUntilNotNavigatingAnymoreAsync(string url, DateTime minLastUpdateTime) {
        if (!Model.UsesRealBrowserOrView) {
            return true;
        }

        if (Model.WebView is { IsWired: false }) {
            Model.Status.Text = string.Format(Properties.Resources.WebViewMustBeWired, MaxSeconds);
            Model.Status.Type = StatusType.Error;
            ApplicationLogger.LogMessage($"Problem when navigating to '{url}'");
            return false;
        }

        ApplicationLogger.LogMessage(Properties.Resources.WaitUntilNotNavigatingAnymore);
        await WaitUntilNotNavigatingAnymoreAsync(minLastUpdateTime, QuickSeconds, IntervalInMilliseconds, DoubleCheckIntervalInMilliseconds);

        if (Model.WebBrowserOrView.IsNavigating) {
            ApplicationLogger.LogMessage(Properties.Resources.WaitLongerUntilNotNavigatingAnymore);
            await WaitUntilNotNavigatingAnymoreAsync(minLastUpdateTime, MaxSeconds - QuickSeconds, LargeIntervalInMilliseconds, DoubleCheckLargeIntervalInMilliseconds);
        }

        if (!Model.WebBrowserOrView.IsNavigating) {
            ApplicationLogger.LogMessage(Properties.Resources.NotNavigatingAnymore);
            return true;
        }

        Model.Status.Text = string.Format(Properties.Resources.WebBrowserStillBusyAfter, MaxSeconds);
        Model.Status.Type = StatusType.Error;
        ApplicationLogger.LogMessage($"Problem when navigating to '{url}'");
        return false;

    }

    private async Task WaitUntilNotNavigatingAnymoreAsync(DateTime minLastUpdateTime, int seconds, int intervalInMilliseconds, int doubleCheckIntervalInMilliseconds) {
        var attempts = seconds * 1000 / intervalInMilliseconds;
        bool again;
        do {
            while ((Model.WebBrowserOrView.LastNavigationStartedAt < minLastUpdateTime || Model.WebBrowserOrView.IsNavigating) && attempts > 0) {
                await Task.Delay(TimeSpan.FromMilliseconds(intervalInMilliseconds));
                attempts--;
                if (attempts == 0) {
                    ApplicationLogger.LogMessage($"Still navigating after {seconds} seconds");
                }
            }

            again = false;
            for (var i = 0; !again && i < 5; i ++) {
                await Task.Delay(TimeSpan.FromMilliseconds(doubleCheckIntervalInMilliseconds));
                attempts--;
                if (attempts == 0) {
                    ApplicationLogger.LogMessage($"Still navigating after {seconds} seconds");
                }
                again = Model.WebBrowserOrView.IsNavigating;
                if (again) {
                    ApplicationLogger.LogMessage(Properties.Resources.NavigatingAgain);
                }
            }
        } while (again && attempts > 0);
    }
}