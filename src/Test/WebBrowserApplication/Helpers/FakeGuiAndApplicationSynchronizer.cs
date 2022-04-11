﻿using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Helpers {
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