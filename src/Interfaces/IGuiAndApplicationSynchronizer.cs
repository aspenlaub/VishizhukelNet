using System;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IGuiAndApplicationSynchronizer<TApplicationModel> where TApplicationModel : IApplicationModel {
        TApplicationModel Model { get; }
        void OnModelDataChanged();
        void IndicateBusy(bool force, Action onCursorChanged);
    }
}
