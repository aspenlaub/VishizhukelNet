using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IGuiAndWebViewApplicationSynchronizer<out TModel>
        : IGuiAndApplicationSynchronizer<TModel>
            where TModel : IWebViewApplicationModelBase {
        Task<TResult> RunScriptAsync<TResult>(IScriptStatement scriptStatement) where TResult : IScriptCallResponse, new();
        Task WaitUntilNotNavigatingAnymoreAsync();
    }
}
