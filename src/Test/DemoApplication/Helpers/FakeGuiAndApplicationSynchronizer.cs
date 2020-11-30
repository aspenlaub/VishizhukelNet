using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Helpers {
    public class FakeGuiAndApplicationSynchronizer : IDemoGuiAndApplicationSynchronizer {
        public DemoApplicationModel Model { get; }
        public DemoApplicationModel LastModelKnownToMe { get; }

        public FakeGuiAndApplicationSynchronizer(DemoApplicationModel model) {
            Model = model;
            LastModelKnownToMe = new DemoApplicationModel();
            OnModelDataChanged();
        }

        public void OnModelDataChanged() {
            LastModelKnownToMe.Alpha.Text = Model.Alpha.Text;
            LastModelKnownToMe.Beta.UpdateSelectables(Model.Beta.Selectables);
            LastModelKnownToMe.Beta.SelectedIndex = Model.Beta.SelectedIndex;
            LastModelKnownToMe.Delta.Text = Model.Delta.Text;
        }

        public void IndicateBusy(bool force) {
        }
    }
}
