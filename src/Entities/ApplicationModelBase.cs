using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities {
    // ReSharper disable once UnusedMember.Global
    public class ApplicationModelBase : IApplicationModelBase {
        public bool IsBusy { get; set; }

        public ITextBox Status { get; set; } = new TextBox { Enabled = false };

        public WindowState WindowState { get; set; }

        public bool UsesRealBrowser { get; set; }
        public IWebBrowserWithDocument WebBrowser { get; } = new WebBrowser();
        public ITextBox WebBrowserUrl { get; } = new TextBox();
        public ITextBox WebBrowserContentSource { get; } = new TextBox();
    }
}
