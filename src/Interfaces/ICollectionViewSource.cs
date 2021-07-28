using System.Collections.Generic;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ICollectionViewSource<T> where T : ICollectionViewSourceEntity {
        IList<T> Rows { get; set; }
    }
}
