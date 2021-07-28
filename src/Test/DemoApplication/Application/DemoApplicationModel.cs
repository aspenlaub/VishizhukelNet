using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Extensions;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application {
    public class DemoApplicationModel : ApplicationModelBase, IDemoApplicationModel {
        public ITextBox Alpha { get; } = new TextBox();
        public ISelector Beta { get; } = new ComboBox();
        public Button Gamma { get; } = new();
        public ITextBox Delta { get; } = new TextBox();
        public IImage Epsilon { get; } = new Image {
            BitmapImage = new MemoryStream(Properties.Resources.Calculator).ToBitmapImage()
        };
        public IImage Zeta { get; } = new Image {
            BitmapImage = new MemoryStream(Properties.Resources.PressToCalculate).ToBitmapImage()
        };
        public IRectangle Eta { get; } = new Rectangle {
            Left = 40, Top = 13, Width = 10, Height = 10, Stroke = new SolidColorBrush(Colors.LimeGreen) { Opacity = 0.5 }, StrokeThickness = 2
        };
        public ICollectionViewSource<IDemoCollectionViewSourceEntity> Theta { get; } = new CollectionViewSource<IDemoCollectionViewSourceEntity> {
            SortProperty = "Date", SortDirection = ListSortDirection.Descending
        };

        public ToggleButton MethodAdd { get; } = new("Method") { IsChecked = true };
        public ToggleButton MethodMultiply { get; } = new("Method");
    }
}
