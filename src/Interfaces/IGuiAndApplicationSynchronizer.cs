namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IGuiAndApplicationSynchronizer {
        ICudotosiApplicationModel Model { get; }
        void OnModelDataChanged();
        void IndicateBusy(bool force);
    }
}
