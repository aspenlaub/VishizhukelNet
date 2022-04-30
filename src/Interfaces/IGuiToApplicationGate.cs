using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Wpf;
using Button = System.Windows.Controls.Button;
using Selector = System.Windows.Controls.Primitives.Selector;
using TextBox = System.Windows.Controls.TextBox;
using ToggleButton = System.Windows.Controls.Primitives.ToggleButton;
// ReSharper disable UnusedMemberInSuper.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IGuiToApplicationGate {
    Task CallbackAsync(Func<Task> action);
    void RegisterAsyncButtonCallback(Button button, Func<Task> action);
    void RegisterAsyncTextBoxCallback(TextBox textBox, Func<string, Task> action);
    void RegisterAsyncSelectorCallback(Selector selector, Func<int, Task> action);
    void WireToggleButtonAndHandler(ToggleButton toggleButton, IToggleButtonHandler handler, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper);
    void WireButtonAndCommand(Button button, ICommand command, IButtonNameToCommandMapper buttonNameToCommandMapper);
    void WireWebView(WebView2 webView);
    void RegisterAsyncDataGridCallback(DataGrid collectionViewSource, Func<IList<ICollectionViewSourceEntity>, Task> action);
}