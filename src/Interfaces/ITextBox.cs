using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ITextBox {
        string Text { get; set; }
        string LabelText { get; set; }
        StatusType Type { get; set; }
        bool Enabled { get; set; }
    }
}
