using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls {
    public class CollectionViewSource<T> : ICollectionViewSource<T>, ICollectionViewSourceObject where T : ICollectionViewSourceEntity {
        public IList<T> Rows { get; set; } = new List<T>();

        public string SortProperty { get; set; } = "";
        public ListSortDirection SortDirection { get; set; } = ListSortDirection.Ascending;

        public object ToObservableCollection() {
            var collection = new ObservableCollection<T>();
            foreach (var row in Rows) {
                collection.Add(row);
            }

            return collection;
        }
    }
}
