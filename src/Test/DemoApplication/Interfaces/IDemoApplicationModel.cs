using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces {
    public interface IDemoApplicationModel : IApplicationModel {
        ITextBox Alpha { get; }
        ISelector Beta { get; }
        Button Gamma { get; }
        ITextBox Delta { get; }
        // ReSharper disable once UnusedMember.Global
        IImage Epsilon { get; }
        ToggleButton MethodAdd { get; }
        ToggleButton MethodMultiply { get; }

        string ErrorMessage { get; set; }
    }
}