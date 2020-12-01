using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ISimpleSelectorHandler {
        Task UpdateSelectableValuesAsync();
        Task SelectedIndexChangedAsync(int selectedIndex);
    }
}
