using System;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
// ReSharper disable UnusedMemberInSuper.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

public interface IDemoCollectionViewSourceEntity : ICollectionViewSourceEntity {
    DateTime Date { get; set; }
    string Name { get; set; }
    double Balance { get; set; }
}