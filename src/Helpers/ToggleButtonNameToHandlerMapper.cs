using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers {
    public class ToggleButtonNameToHandlerMapper : IToggleButtonNameToHandlerMapper {
        private readonly IDictionary<string, IToggleButtonHandler> vToggleButtonNameToHandlerDictionary = new Dictionary<string, IToggleButtonHandler>();

        public void Register(string name, IToggleButtonHandler handler) {
            vToggleButtonNameToHandlerDictionary[name] = handler;
        }

        public IToggleButtonHandler HandlerForToggleButton(string name) {
            return !vToggleButtonNameToHandlerDictionary.ContainsKey(name) ? null : vToggleButtonNameToHandlerDictionary[name];
        }
    }
}
