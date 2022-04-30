namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IButtonNameToCommandMapper {
    void Register(string name, ICommand command);
    // ReSharper disable once UnusedMember.Global
    ICommand CommandForButton(string name);
}