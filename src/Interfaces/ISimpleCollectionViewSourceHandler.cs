using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface ISimpleCollectionViewSourceHandler<TCollectionViewSourceEntity> where TCollectionViewSourceEntity : ICollectionViewSourceEntity {
    Task CollectionChangedAsync(IList<TCollectionViewSourceEntity> items);

    IList<TCollectionViewSourceEntity> DeserializeJson(string json);
}