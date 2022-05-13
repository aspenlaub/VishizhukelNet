using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;

public class ScriptStatement : IScriptStatement {
    private string PrivateStatement = "";

    public string Statement {
        get => PrivateStatement;
        set {
            PrivateStatement = value;
            if (NoSuccessErrorMessage != Properties.Resources.ScriptCallFailed) {
                return;
            }

            var shortStatement = PrivateStatement;
            if (shortStatement.Length > 20) {
                shortStatement = shortStatement.Substring(0, 20) + "..";
            }
            NoSuccessErrorMessage = Properties.Resources.ScriptCallFailed + ": " + shortStatement;
        }
    }

    public string NoSuccessErrorMessage { get; set; } = Properties.Resources.ScriptCallFailed;
    public string InconclusiveErrorMessage { get; set; } = "";
}