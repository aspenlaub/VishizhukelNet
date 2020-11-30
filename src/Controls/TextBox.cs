using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls {
    public class TextBox : ITextBox {
        public string Text { get; set; }
        public string LabelText { get; set; }
        public StatusType Type { get; set; }
        public bool Enabled { get; set; }

        public TextBox() {
            Text = "";
            LabelText = "";
            Type = StatusType.None;
            Enabled = true;
        }
    }
}
