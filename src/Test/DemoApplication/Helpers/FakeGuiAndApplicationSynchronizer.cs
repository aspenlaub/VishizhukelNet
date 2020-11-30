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
            LastModelKnownToMe.A.Text = Model.A.Text;
            LastModelKnownToMe.B.UpdateSelectables(Model.B.Selectables);
            LastModelKnownToMe.B.SelectedIndex = Model.B.SelectedIndex;
            LastModelKnownToMe.D.Text = Model.D.Text;
        }

        public void IndicateBusy(bool force) {
        }
    }
}
