using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ISelector {
        int SelectedIndex { get; set; }
        List<Selectable> Selectables { get; }
        string LabelText { get; set; }
        bool SelectionMade { get; }
        Selectable SelectedItem { get; }
        bool AreSelectablesIdentical(IList<Selectable> selectables);
        void UpdateSelectables(IList<Selectable> selectables);
    }
}