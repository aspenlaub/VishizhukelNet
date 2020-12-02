﻿using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Handlers {
    public class AlphaTextHandler : ISimpleTextHandler {
        private readonly IDemoApplicationModel vModel;
        private readonly IGuiAndAppHandler vGuiAndAppHandler;
        private readonly ISimpleSelectorHandler vBetaSelectorHandler;
        private readonly ISimpleTextHandler vDeltaTextHandler;

        public AlphaTextHandler(IDemoApplicationModel model, IGuiAndAppHandler guiAndAppHandler, ISimpleSelectorHandler betaSelectorHandler, ISimpleTextHandler deltaTextHandler) {
            vModel = model;
            vGuiAndAppHandler = guiAndAppHandler;
            vBetaSelectorHandler = betaSelectorHandler;
            vDeltaTextHandler = deltaTextHandler;
        }

        public async Task TextChangedAsync(string text) {
            if (vModel.Alpha.Text == text) { return; }

            vModel.Alpha.Text = text;
            vModel.Alpha.Type = uint.TryParse(text, out _) ? StatusType.None : StatusType.Error;
            await vBetaSelectorHandler.UpdateSelectableValuesAsync();
            await vDeltaTextHandler.TextChangedAsync("");
            await vGuiAndAppHandler.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }
    }
}
