using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ICollectionViewSource {
        Type EntityType { get; set; }
        IList<ICollectionViewSourceEntity> Items { get; set; }

        string SortProperty { get; set; }
        ListSortDirection SortDirection { get; set; }
    }
}
