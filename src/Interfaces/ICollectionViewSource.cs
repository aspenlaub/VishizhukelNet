using System;
using System.Collections.Generic;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ICollectionViewSource {
        Type EntityType { get; set; }
        IList<ICollectionViewSourceEntity> Items { get; set; }
    }
}
