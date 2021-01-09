using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ITashSelectorHandler<in TModel> where TModel : IApplicationModel {
        Task ProcessSelectComboOrResetTaskAsync(ITashTaskHandlingStatus<TModel> status);
    }
}
