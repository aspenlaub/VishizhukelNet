using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces {
    public interface IBetaSelectorHandler {
        Task UpdateSelectableBetaValuesAsync();
        Task SelectedBetaIndexChangedAsync(int selectedBetaIndex);
    }
}
