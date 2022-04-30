using System.Threading.Tasks;
// ReSharper disable UnusedMemberInSuper.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IWebBrowserOrViewNavigationHelper {
    Task<bool> NavigateToUrlAsync(string url);
}