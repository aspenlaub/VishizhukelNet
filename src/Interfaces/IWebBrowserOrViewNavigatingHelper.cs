using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IWebBrowserOrViewNavigatingHelper {
    Task<bool> WaitUntilNotNavigatingAnymoreAsync(string url);
}