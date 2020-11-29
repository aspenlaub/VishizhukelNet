using System;
using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IGuiAndAppHandler {
        Task EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        void IndicateBusy(bool force, Action onCursorChanged);
    }
}
