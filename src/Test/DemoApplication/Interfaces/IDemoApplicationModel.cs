﻿using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces {
    public interface IDemoApplicationModel : IApplicationModel {
        ITextBox Alpha { get; }
        ISelector Beta { get; }
        Button Gamma { get; }
        ITextBox Delta { get; }
        IImage Epsilon { get; }
        ToggleButton MethodAdd { get; }
        ToggleButton MethodMultiply { get; }
        IImage Zeta { get; }
        IRectangle Eta { get; }
        ICollectionViewSource<IDemoCollectionViewSourceEntity> Theta { get; }
    }
}