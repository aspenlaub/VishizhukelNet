using System;
using System.Collections.Generic;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface ICollectionViewSource<TCollectionViewSourceEntity> where TCollectionViewSourceEntity : ICollectionViewSourceEntity {
    Type EntityType { get; set; }
    IList<TCollectionViewSourceEntity> Items { get; set; }
}