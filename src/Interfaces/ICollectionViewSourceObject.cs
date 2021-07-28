using System.ComponentModel;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ICollectionViewSourceObject {
        public object ToObservableCollection();

        string SortProperty { get; set; }
        ListSortDirection SortDirection { get; set; }
    }
}
