using System.Windows;
using System.Windows.Data;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.GUI {
    public class ViewSources {
        public CollectionViewSource DemoDataGridViewSource;

        public ViewSources(FrameworkElement window) {
            DemoDataGridViewSource = window.FindResource(nameof(DemoDataGridViewSource)) as CollectionViewSource;
        }
    }
}
