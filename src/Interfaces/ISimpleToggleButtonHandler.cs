using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ISimpleToggleButtonHandler {
        Task ToggledAsync(bool isChecked);
    }
}
