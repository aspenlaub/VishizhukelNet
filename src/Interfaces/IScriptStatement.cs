namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IScriptStatement {
    string Statement { get; set; }
    string NoSuccessErrorMessage { get; set; }
    string InconclusiveErrorMessage { get; set; }
}