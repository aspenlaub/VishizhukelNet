using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Handlers {
    public class ApplicationHandlers : IApplicationHandlers {
        public ISimpleTextHandler WebBrowserUrlTextHandler { get; set; }
    }
}
