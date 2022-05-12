using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;

public class ScriptStatement : IScriptStatement {
    public string Statement { get; set;  } = "";
    public string NoSuccessErrorMessage { get; set; } = Properties.Resources.ScriptCallFailed;
    public string InconclusiveErrorMessage { get; set; } = "";
}