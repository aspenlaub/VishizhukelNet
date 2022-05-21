using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IGuiAndAppHandler {
    Task EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    Task SyncGuiAndAppAsync();
    // ReSharper disable once UnusedMemberInSuper.Global
    void IndicateBusy(bool force);

    IApplicationModelBase GetModel();

    Task OnWebViewSourceChangedAsync(string uri);
    Task OnWebViewNavigationCompletedAsync(string contentSource, bool isSuccess);
    Task<TResult> RunScriptAsync<TResult>(IScriptStatement scriptStatement, bool mayFail, bool maySucceed) where TResult : IScriptCallResponse, new();
    Task WaitUntilNotNavigatingAnymoreAsync();
}