using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces {
    public interface IApplicationHandlers {
        ISimpleTextHandler WebBrowserUrlTextHandler { get; set; }
        ISimpleTextHandler WebBrowserContentSourceTextHandler { get; set; }
    }
}
