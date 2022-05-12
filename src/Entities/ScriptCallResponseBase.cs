using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;

public class ScriptCallResponseBase : IScriptCallResponse {
    public YesNoInconclusive Success { get; set; }
    public string ErrorMessage { get; set; } = "";
}