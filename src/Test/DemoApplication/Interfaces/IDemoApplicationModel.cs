using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces {
    public interface IDemoApplicationModel {
        ITextBox A { get; }
        ISelector B { get; }
        Button C { get; }
        ITextBox D { get; }
    }
}