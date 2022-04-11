using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ITashVerifyAndSetHandler<in TModel> where TModel : IApplicationModelBase {
        Task ProcessVerifyGetOrSetValueOrLabelTaskAsync(ITashTaskHandlingStatus<TModel> status, bool verify, bool set, bool label, bool combined);
        Task ProcessVerifyWhetherEnabledTaskAsync(ITashTaskHandlingStatus<TModel> status);
        Task ProcessVerifyNumberOfItemsTaskAsync(ITashTaskHandlingStatus<TModel> status);
    }
}
