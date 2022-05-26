using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IGuiAndWebViewAppHandler<TModel> : IGuiAndAppHandler<TModel> where TModel : IWebViewApplicationModelBase {
        Task OnWebViewSourceChangedAsync(string uri);
        Task OnWebViewNavigationCompletedAsync(string contentSource, bool isSuccess);
        Task<TResult> RunScriptAsync<TResult>(IScriptStatement scriptStatement, bool mayFail, bool maySucceed) where TResult : IScriptCallResponse, new();
        Task WaitUntilNotNavigatingAnymoreAsync();
    }
}
