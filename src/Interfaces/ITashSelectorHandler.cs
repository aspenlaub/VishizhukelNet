using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ITashSelectorHandler<in TModel> where TModel : IApplicationModel {
        Task ProcessSelectComboOrResetTaskAsync(ITashTaskHandlingStatus<TModel> status);
    }
}
