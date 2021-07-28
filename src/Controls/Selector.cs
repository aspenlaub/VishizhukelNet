using System.Collections.Generic;
using System.Linq;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls {
    public class Selector : ISelector {
        public int SelectedIndex { get; set; } = -1;
        public List<Selectable> Selectables { get; private set; } = new();
        public string LabelText { get; set; } = "";

        public bool SelectionMade => SelectedIndex >= 0;
        public Selectable SelectedItem => SelectionMade ? Selectables[SelectedIndex] : null;

        public bool AreSelectablesIdentical(IList<Selectable> selectables) {
            return Selectables.Count == selectables.Count && Enumerable.Range(0, Selectables.Count).All(i => Selectables[i].Guid == selectables[i].Guid && Selectables[i].Name == selectables[i].Name);
        }

        public void UpdateSelectables(IList<Selectable> selectables) {
            var selectedGuid = SelectedIndex >= 0 && SelectedIndex < Selectables.Count ? Selectables[SelectedIndex].Guid : "";
            Selectables = selectables.ToList();
            SelectedIndex = Selectables.FindIndex(s => s.Guid == selectedGuid);
        }
    }
}
