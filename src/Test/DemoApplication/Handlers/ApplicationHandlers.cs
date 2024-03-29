﻿using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;

public class ApplicationHandlers : IApplicationHandlers {
    public ISimpleTextHandler AlphaTextHandler { get; set; }
    public ISimpleSelectorHandler BetaSelectorHandler { get; set; }
    public ISimpleTextHandler DeltaTextHandler { get; set; }
    public ISimpleCollectionViewSourceHandler ThetaHandler { get; set; }

    public IToggleButtonHandler MethodAddHandler { get; set; }
    public IToggleButtonHandler MethodMultiplyHandler { get; set; }
}