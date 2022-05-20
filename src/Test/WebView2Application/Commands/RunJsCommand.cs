using System;
using System.Text.Json;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Commands;

public class RunJsCommand : ICommand {
    private readonly IApplicationModel Model;
    private readonly IGuiAndAppHandler GuiAndAppHandler;

    public RunJsCommand(IApplicationModel model, IGuiAndAppHandler guiAndAppHandler) {
        Model = model;
        GuiAndAppHandler = guiAndAppHandler;
    }

    public async Task ExecuteAsync() {
        if (!Model.RunJs.Enabled) {
            return;
        }

        var scriptCallResponse = new ScriptCallResponseBase {
            Success = new YesNoInconclusive { Inconclusive = false, YesNo = true }
        };
        var statement = "(function() { "
                                 + "alert('A script has been run: ' + document.head.children[document.head.children.length - 1].outerHTML); "
                                 + "return " + JsonSerializer.Serialize(scriptCallResponse)
                                 + "})();";
        const string inconclusiveErrorMessage = "Script result is inconclusive";
        const string noSuccessErrorMessage = "Script call failed";
        var scriptStatement = new ScriptStatement { Statement = statement, InconclusiveErrorMessage = inconclusiveErrorMessage, NoSuccessErrorMessage = noSuccessErrorMessage };
        await GuiAndAppHandler.RunScriptAsync<ScriptCallResponseBase>(scriptStatement, false, true);
    }

    public async Task<bool> ShouldBeEnabledAsync() {
        var enabled = Model.WebViewUrl.Text.StartsWith("http", StringComparison.InvariantCulture);
        return await Task.FromResult(enabled);
    }
}