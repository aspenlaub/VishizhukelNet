using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ITashHandler<in TModel> where TModel : IApplicationModelBase {
        Task<bool> UpdateTashStatusAndReturnIfIsWorkAsync(ITashTaskHandlingStatus<TModel> status);
        Task ProcessTashAsync(ITashTaskHandlingStatus<TModel> status);
    }
}
