using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface ISimpleCollectionViewSourceHandler {
    Task CollectionChangedAsync(IList<ICollectionViewSourceEntity> items);

    IList<ICollectionViewSourceEntity> DeserializeJson(string json);
}