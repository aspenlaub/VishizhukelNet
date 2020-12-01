namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IGuiAndApplicationSynchronizer<out TApplicationModel> where TApplicationModel : IApplicationModel {
        // ReSharper disable once UnusedMemberInSuper.Global
        TApplicationModel Model { get; }
        void OnModelDataChanged();
        void IndicateBusy(bool force);
    }
}
