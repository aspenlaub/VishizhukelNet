﻿using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers {
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
            LastModelKnownToMe.Alpha.Text = Model.Alpha.Text;
            LastModelKnownToMe.Beta.UpdateSelectables(Model.Beta.Selectables);
            LastModelKnownToMe.Beta.SelectedIndex = Model.Beta.SelectedIndex;
            LastModelKnownToMe.Delta.Text = Model.Delta.Text;
        }

        public void IndicateBusy(bool force) {
        }
    }
}
