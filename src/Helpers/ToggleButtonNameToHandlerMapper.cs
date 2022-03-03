using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers {
    public class ToggleButtonNameToHandlerMapper : IToggleButtonNameToHandlerMapper {
        private readonly IDictionary<string, IToggleButtonHandler> ToggleButtonNameToHandlerDictionary = new Dictionary<string, IToggleButtonHandler>();

        public void Register(string name, IToggleButtonHandler handler) {
            ToggleButtonNameToHandlerDictionary[name] = handler;
        }

        public IToggleButtonHandler HandlerForToggleButton(string name) {
            return !ToggleButtonNameToHandlerDictionary.ContainsKey(name) ? null : ToggleButtonNameToHandlerDictionary[name];
        }
    }
}
