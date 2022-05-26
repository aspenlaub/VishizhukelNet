using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;

// ReSharper disable once UnusedMember.Global
public class ApplicationModelBase : IApplicationModelBase {
    public bool IsBusy { get; set; }

    public ITextBox Status { get; set; } = new TextBox { Enabled = false };

    public WindowState WindowState { get; set; }
}