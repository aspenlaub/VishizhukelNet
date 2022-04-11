using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IWebBrowserNavigationHelper {
        Task<bool> NavigateToUrlAsync(string url);
        int MaxSeconds { get; }
    }
}
