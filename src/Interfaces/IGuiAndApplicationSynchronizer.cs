using System;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IGuiAndApplicationSynchronizer {
        IApplicationModel Model { get; }
        void OnModelDataChanged();
        void IndicateBusy(bool force, Action onCursorChanged);
    }
}
