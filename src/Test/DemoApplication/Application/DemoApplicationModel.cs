using System.IO;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Extensions;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application {
    public class DemoApplicationModel : ApplicationModelBase, IDemoApplicationModel {
        public ITextBox Alpha { get; } = new TextBox();
        public ISelector Beta { get; } = new ComboBox();
        public Button Gamma { get; } = new Button();
        public ITextBox Delta { get; } = new TextBox();
        public IImage Epsilon { get; } = new Image {
            BitmapImage = new MemoryStream(Properties.Resources.Calculator).ToBitmapImage()
        };
        public ToggleButton MethodAdd { get; } = new ToggleButton("Method") { IsChecked = true };
        public ToggleButton MethodMultiply { get; } = new ToggleButton("Method");

        public string ErrorMessage { get; set; } = "";

        public override bool IsModelErroneous(out string errorMessage) {
            errorMessage = ErrorMessage;
            return !string.IsNullOrWhiteSpace(errorMessage);
        }
    }
}
