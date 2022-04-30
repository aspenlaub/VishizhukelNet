using System.Threading.Tasks;
// ReSharper disable UnusedMemberInSuper.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IToggleButtonHandler {
    bool Unchanged(bool isChecked);
    Task ToggledAsync(bool isChecked);
    void SetChecked(bool isChecked);
    bool IsChecked();
}