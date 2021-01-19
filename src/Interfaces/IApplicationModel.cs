using System.Windows;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IApplicationModel : IBusy {
        bool IsModelErroneous(out string errorMessage);

        WindowState WindowState { get; set; }
    }
}
