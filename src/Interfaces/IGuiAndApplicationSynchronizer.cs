using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IGuiAndApplicationSynchronizer<out TApplicationModel> where TApplicationModel : IApplicationModelBase {
    // ReSharper disable once UnusedMemberInSuper.Global
    TApplicationModel Model { get; }
    Task OnModelDataChangedAsync();
    void IndicateBusy(bool force);

    void OnWebBrowserLoadCompleted();
    Task<TResult> RunScriptAsync<TResult>(IScriptStatement scriptStatement) where TResult : IScriptCallResponse, new();
}