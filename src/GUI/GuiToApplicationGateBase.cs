﻿using System;
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
            button.Click += async (_, _) => await CallbackAsync(() => action());
        }

        public void RegisterAsyncTextBoxCallback(TextBox textBox, Func<string, Task> action) {
            textBox.TextChanged += async (_, _) => await CallbackAsync(() => action(textBox.Text));
        }

        public void RegisterAsyncSelectorCallback(Selector selector, Func<int, Task> action) {
            selector.SelectionChanged += async (_, _) => await CallbackAsync(() => action(selector.SelectedIndex));
        }

        public void WireToggleButtonAndHandler(ToggleButton toggleButton, IToggleButtonHandler handler, IToggleButtonNameToHandlerMapper toggleButtonNameToHandlerMapper) {
            toggleButtonNameToHandlerMapper.Register(toggleButton.Name, handler);
            toggleButton.Click += async (_, _) => await CallbackAsync(() => handler.ToggledAsync(toggleButton.IsChecked == true));
        }

        public void WireButtonAndCommand(Button button, ICommand command, IButtonNameToCommandMapper buttonNameToCommandMapper) {
            buttonNameToCommandMapper.Register(button.Name, command);
            RegisterAsyncButtonCallback(button, command.ExecuteAsync);
        }
    }
}
