﻿using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class DemoHandlers : IDemoHandlers {
        public ISimpleTextHandler AlphaTextHandler { get; set; }
        public ISimpleSelectorHandler BetaSelectorHandler { get; set; }
        public ISimpleTextHandler DeltaTextHandler { get; set; }
        public ISimpleToggleButtonHandler MethodAddHandler { get; set; }
        public ISimpleToggleButtonHandler MethodMultiplyHandler { get; set; }
    }
}
