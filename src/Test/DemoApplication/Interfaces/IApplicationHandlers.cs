using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces {
    public interface IApplicationHandlers {
        ISimpleTextHandler AlphaTextHandler { get; set; }
        ISimpleSelectorHandler BetaSelectorHandler { get; set; }
        // ReSharper disable once UnusedMemberInSuper.Global
        ISimpleTextHandler DeltaTextHandler { get; set; }
        ISimpleCollectionViewSourceHandler ThetaHandler { get; set; }

        IToggleButtonHandler MethodAddHandler { get; set; }
        IToggleButtonHandler MethodMultiplyHandler { get; set; }
    }
}
