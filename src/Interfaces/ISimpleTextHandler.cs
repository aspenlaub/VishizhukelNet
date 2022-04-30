using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface ISimpleTextHandler {
    Task TextChangedAsync(string text);
}