using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ITashTimer<in TModel> where TModel : IApplicationModel {
        Task<bool> ConnectAndMakeTashRegistrationReturnSuccessAsync(string windowTitle);
        void CreateAndStartTimer(ITashTaskHandlingStatus<TModel> status);
        Task StopTimerAndConfirmDeadAsync(bool ignoreSocketException);
    }
}
