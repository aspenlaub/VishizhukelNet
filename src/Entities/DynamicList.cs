using System;
using System.Collections;
using System.Collections.Generic;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities {
    public class DynamicList {
        private readonly Type _EntityType;
        private readonly IList _List;

        public DynamicList(Type entityType) {
            _EntityType = entityType;
            var listType = typeof(List<>).MakeGenericType(entityType);
            _List = (IList)Activator.CreateInstance(listType);
        }

        public void Add(object item) {
            if (item.GetType() != _EntityType) { return; }

            _List.Add(item);
        }

        public void AddRange(IEnumerable<object> items) {
            foreach (var item in items) {
                Add(item);
            }
        }

        public IList ToList() {
            return _List;
        }
    }
}
