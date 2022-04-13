using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Helpers {
    public class FakeGuiAndApplicationSynchronizer : IGuiAndApplicationSynchronizer {
        public IApplicationModel Model { get; }
        public ApplicationModel LastModelKnownToMe { get; }

        public FakeGuiAndApplicationSynchronizer(IApplicationModel model) {
            Model = model;
            LastModelKnownToMe = new ApplicationModel();
            SetLastModelKnownToMeGreeks();
        }

        public async Task OnModelDataChangedAsync() {
            SetLastModelKnownToMeGreeks();
            await Task.CompletedTask;
        }

        public void SetLastModelKnownToMeGreeks() {
        }

        public void IndicateBusy(bool force) {
        }

        public void OnWebBrowserLoadCompleted() {
        }
    }
}
