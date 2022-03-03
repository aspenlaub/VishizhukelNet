﻿using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers {
    public class ButtonNameToCommandMapper : IButtonNameToCommandMapper {
        private readonly IDictionary<string, ICommand> ButtonNameToCommandDictionary = new Dictionary<string, ICommand>();

        public void Register(string name, ICommand command) {
            ButtonNameToCommandDictionary[name] = command;
        }

        public ICommand CommandForButton(string name) {
            return !ButtonNameToCommandDictionary.ContainsKey(name) ? null : ButtonNameToCommandDictionary[name];
        }
    }
}
