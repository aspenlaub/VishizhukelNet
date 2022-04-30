using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface ICommand {
    Task ExecuteAsync();
    Task<bool> ShouldBeEnabledAsync();
}