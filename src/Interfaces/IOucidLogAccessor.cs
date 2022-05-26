using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IOucidLogAccessor {
    Task WriteOucidAsync(string oucid, OucidResponses oucidResponses, IErrorsAndInfos errorsAndInfos);
    Task<OucidResponse> ReadAndDeleteOucidAsync(string oucid, IErrorsAndInfos errorsAndInfos);
    string AppendOucidToUrl(string url, string oucid, IErrorsAndInfos errorsAndInfos);
    Task<string> GenerateOucidAsync(IErrorsAndInfos errorsAndInfos);
}