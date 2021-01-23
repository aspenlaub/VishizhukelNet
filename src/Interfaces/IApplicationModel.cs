using System.Windows;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IApplicationModel : IBusy {
        ITextBox Status { get; }

        WindowState WindowState { get; set; }
    }
}
