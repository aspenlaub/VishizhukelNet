using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers;

public class ToggleButtonNameToHandlerMapper : IToggleButtonNameToHandlerMapper {
    private readonly IDictionary<string, IToggleButtonHandler> _ToggleButtonNameToHandlerDictionary = new Dictionary<string, IToggleButtonHandler>();

    public void Register(string name, IToggleButtonHandler handler) {
        _ToggleButtonNameToHandlerDictionary[name] = handler;
    }

    public IToggleButtonHandler HandlerForToggleButton(string name) {
        return !_ToggleButtonNameToHandlerDictionary.ContainsKey(name) ? null : _ToggleButtonNameToHandlerDictionary[name];
    }
}