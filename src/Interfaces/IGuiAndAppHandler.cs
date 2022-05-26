using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IGuiAndAppHandler<out TModel> where TModel : IApplicationModelBase {
    Task EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    Task SyncGuiAndAppAsync();
    // ReSharper disable once UnusedMemberInSuper.Global
    void IndicateBusy(bool force);

    TModel GetModel();
}