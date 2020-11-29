namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IButtonNameToCommandMapper {
        void Register(string name, ICommand command);
        ICommand CommandForButton(string name);
    }
}
