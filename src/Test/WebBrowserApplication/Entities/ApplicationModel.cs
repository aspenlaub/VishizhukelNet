using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Entities;

public class ApplicationModel : ApplicationModelBase<WebBrowser>, IApplicationModel {
    public Button GoToUrl { get; } = new();
}