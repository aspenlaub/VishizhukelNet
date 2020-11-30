using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IGuiToApplicationGate {
        Task CallbackAsync(Func<Task> action);
        void RegisterAsyncButtonCallback(Button button, Func<Task> action);
        void RegisterAsyncTextBoxCallback(TextBox textBox, Func<string, Task> action);
        void RegisterAsyncSelectorCallback(Selector selector, Func<int, Task> action);
        void WireButtonAndCommand(Button button, ICommand command, IButtonNameToCommandMapper buttonNameToCommandMapper);
    }
}
