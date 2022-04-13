using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Interfaces {
    public interface IApplicationModel : IApplicationModelBase {
        Button GoToUrl { get; }
    }
}