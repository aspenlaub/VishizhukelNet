namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IToggleButtonNameToHandlerMapper {
    void Register(string name, IToggleButtonHandler handler);
    // ReSharper disable once UnusedMember.Global
    IToggleButtonHandler HandlerForToggleButton(string name);
}