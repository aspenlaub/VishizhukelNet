using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Application {
    public class DemoApplicationModel : IApplicationModel, IDemoApplicationModel {
        public bool IsBusy { get; set ; }

        public ITextBox A { get; } = new TextBox();
        public ISelector B { get; } = new ComboBox();
        public Button C { get; } = new Button();
        public ITextBox D { get; } = new TextBox();
    }
}
