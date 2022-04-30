using System.Threading.Tasks;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface ITashSelectorHandler<in TModel> where TModel : IApplicationModelBase {
    Task ProcessSelectComboOrResetTaskAsync(ITashTaskHandlingStatus<TModel> status);
}