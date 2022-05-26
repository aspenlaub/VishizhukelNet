﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Newtonsoft.Json;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers;

public class ThetaHandler<TModel> : ISimpleCollectionViewSourceHandler where TModel : IApplicationModel {
    private readonly IApplicationModel Model;
    private readonly IGuiAndAppHandler<TModel> GuiAndAppHandler;

    public ThetaHandler(IApplicationModel model, IGuiAndAppHandler<TModel> guiAndAppHandler) {
        Model = model;
        GuiAndAppHandler = guiAndAppHandler;
    }

    public async Task CollectionChangedAsync(IList<ICollectionViewSourceEntity> items) {
        Model.Theta.Items.Clear();
        foreach (var item in items.Where(item => item.GetType() == Model.Theta.EntityType)) {
            Model.Theta.Items.Add(item);
        }
        await GuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
    }

    public IList<ICollectionViewSourceEntity> DeserializeJsonObject(string text) {
        var list = JsonConvert.DeserializeObject<List<DemoCollectionViewSourceEntity>>(text);
        return list == null ? new List<ICollectionViewSourceEntity>() : list.Cast<ICollectionViewSourceEntity>().ToList();
    }
}