using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;

public class ButtonNameToCommandMapper : IButtonNameToCommandMapper {
    private readonly IDictionary<string, ICommand> _ButtonNameToCommandDictionary = new Dictionary<string, ICommand>();

    public void Register(string name, ICommand command) {
        _ButtonNameToCommandDictionary[name] = command;
    }

    public ICommand CommandForButton(string name) {
        return !_ButtonNameToCommandDictionary.ContainsKey(name) ? null : _ButtonNameToCommandDictionary[name];
    }
}