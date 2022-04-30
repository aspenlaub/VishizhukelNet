namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;

public class ToggleButton {
    public string GroupName { get; }
    public bool Enabled { get; set; }
    public bool IsChecked { get; set; }

    public ToggleButton(string groupName) {
        GroupName = groupName;
        Enabled = true;
    }
}