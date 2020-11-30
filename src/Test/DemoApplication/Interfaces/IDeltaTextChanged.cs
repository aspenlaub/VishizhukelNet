using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces {
    public interface IDeltaTextChanged {
        Task DeltaTextChangedAsync(string text);
    }
}
