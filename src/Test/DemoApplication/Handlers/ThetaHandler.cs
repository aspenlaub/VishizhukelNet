using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;

public class ThetaHandler<TModel> : ISimpleCollectionViewSourceHandler where TModel : IApplicationModel {
    private readonly IApplicationModel _Model;
    private readonly IGuiAndAppHandler<TModel> _GuiAndAppHandler;

    public ThetaHandler(IApplicationModel model, IGuiAndAppHandler<TModel> guiAndAppHandler) {
        _Model = model;
        _GuiAndAppHandler = guiAndAppHandler;
    }

    public async Task CollectionChangedAsync(IList<ICollectionViewSourceEntity> items) {
        _Model.Theta.Items.Clear();
        foreach (var item in items.Where(item => item.GetType() == _Model.Theta.EntityType)) {
            _Model.Theta.Items.Add(item);
        }
        await _GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }

    public IList<ICollectionViewSourceEntity> DeserializeJsonObject(string text) {
        var list = JsonSerializer.Deserialize<List<DemoCollectionViewSourceEntity>>(text);
        return list == null ? new List<ICollectionViewSourceEntity>() : list.Cast<ICollectionViewSourceEntity>().ToList();
    }
}