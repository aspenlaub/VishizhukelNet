using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces {
    public interface IDemoApplicationModel {
        ITextBox Alpha { get; }
        ISelector Beta { get; }
        Button Gamma { get; }
        ITextBox Delta { get; }
    }
}