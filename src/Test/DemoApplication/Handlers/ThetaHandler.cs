using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Newtonsoft.Json;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class ThetaHandler : ISimpleCollectionViewSourceHandler {
        private readonly IDemoApplicationModel vModel;
        private readonly IGuiAndAppHandler vGuiAndAppHandler;

        public ThetaHandler(IDemoApplicationModel model, IGuiAndAppHandler guiAndAppHandler) {
            vModel = model;
            vGuiAndAppHandler = guiAndAppHandler;
        }

        public async Task CollectionChangedAsync(IList<ICollectionViewSourceEntity> items) {
            vModel.Theta.Items.Clear();
            foreach (var item in items.Where(item => item.GetType() == vModel.Theta.EntityType)) {
                vModel.Theta.Items.Add(item);
            }
            await vGuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }

        public IList<ICollectionViewSourceEntity> DeserializeJsonObject(string text) {
            var list = JsonConvert.DeserializeObject<List<DemoCollectionViewSourceEntity>>(text);
            return list == null ? new List<ICollectionViewSourceEntity>() : list.Cast<ICollectionViewSourceEntity>().ToList();
        }
    }
}
