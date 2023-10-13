using System;
using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;

public class CollectionViewSource<TCollectionViewSourceEntity> : ICollectionViewSource<TCollectionViewSourceEntity>
        where TCollectionViewSourceEntity : ICollectionViewSourceEntity {
    public Type EntityType { get; set; }

    public IList<TCollectionViewSourceEntity> Items { get; set; } = new List<TCollectionViewSourceEntity>();
}