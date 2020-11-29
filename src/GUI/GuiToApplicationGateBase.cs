using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI {
    public abstract class GuiToApplicationGateBase<TApplication> : IGuiToApplicationGate where TApplication : IGuiAndAppHandler {
        protected readonly IBusy Busy;
        protected readonly TApplication Application;

        protected GuiToApplicationGateBase(IBusy busy, TApplication application) {
            Busy = busy;
            Application = application;
        }

        public async Task CallbackAsync(Func<Task> action) {
            if (Busy.IsBusy) { return; }

            Busy.IsBusy = true;
            await action();
            Busy.IsBusy = false;
            await Application.EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        }

        public void RegisterAsyncButtonCallback(Button button, Func<Task> action) {
            button.Click += async (s, e) => await CallbackAsync(() => action());
        }

        public void RegisterAsyncTextBoxCallback(TextBox textBox, Func<string, Task> action) {
            textBox.TextChanged += async (s, e) => await CallbackAsync(() => action(textBox.Text));
        }

        public void RegisterAsyncSelectorCallback(Selector selector, Func<int, Task> action) {
            selector.SelectionChanged += async (s, e) => await CallbackAsync(() => action(selector.SelectedIndex));
        }
    }
}
