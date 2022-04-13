using System.Windows;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IApplicationModelBase : IBusy {
        ITextBox Status { get; }

        WindowState WindowState { get; set; }

        bool UsesRealBrowser { get; set; }
        IWebBrowserWithDocument WebBrowser { get; }
        ITextBox WebBrowserUrl { get; }
        ITextBox WebBrowserContentSource { get; }
    }
}
