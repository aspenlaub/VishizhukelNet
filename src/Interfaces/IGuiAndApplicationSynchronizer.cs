using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IGuiAndApplicationSynchronizer<out TModel> where TModel : IApplicationModelBase {
    // ReSharper disable once UnusedMemberInSuper.Global
    TModel Model { get; }
    Task OnModelDataChangedAsync();
    void IndicateBusy(bool force);
}