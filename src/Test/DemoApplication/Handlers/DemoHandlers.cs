using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class DemoHandlers : IDemoHandlers {
        public IBetaSelectorHandler BetaSelectorHandler { get; set; }
    }
}
