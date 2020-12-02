using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces {
    public interface IDemoHandlers {
        ISimpleTextHandler AlphaTextHandler { get; set; }
        ISimpleSelectorHandler BetaSelectorHandler { get; set; }
        // ReSharper disable once UnusedMemberInSuper.Global
        ISimpleTextHandler DeltaTextHandler { get; set; }
        ISimpleToggleButtonHandler MethodAddHandler { get; set; }
        ISimpleToggleButtonHandler MethodMultiplyHandler { get; set; }
    }
}
