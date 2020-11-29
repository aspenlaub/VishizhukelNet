using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Button = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities.Button;
using WindowsTextBox = System.Windows.Controls.TextBox;
using WindowsButton = System.Windows.Controls.Button;
using WindowsWebBrowser = System.Windows.Controls.WebBrowser;
using WindowsSelector = System.Windows.Controls.Primitives.Selector;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI {
    public abstract class GuiAndApplicationSynchronizerBase<TApplicationModel, TWindow> where TApplicationModel : IApplicationModel {
        protected readonly TWindow Window;
        protected readonly Dictionary<PropertyInfo, FieldInfo> ModelToWindowPropertyMapping, ModelToWindowLabelPropertyMapping;

        public TApplicationModel Model { get; }

        protected GuiAndApplicationSynchronizerBase(TApplicationModel model, TWindow window) {
            Model = model;
            Window = window;
            ModelToWindowPropertyMapping = new Dictionary<PropertyInfo, FieldInfo>();
            ModelToWindowLabelPropertyMapping = new Dictionary<PropertyInfo, FieldInfo>();
            var modelProperties = typeof(TApplicationModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var windowFields = typeof(TWindow).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var modelProperty in modelProperties) {
                var windowField = windowFields.FirstOrDefault(p => p.Name == modelProperty.Name);
                if (windowField != null) {
                    ModelToWindowPropertyMapping[modelProperty] = windowField;
                }

                var labelField = windowFields.FirstOrDefault(p => p.Name == modelProperty.Name + "Label");
                if (labelField == null) { continue; }

                ModelToWindowLabelPropertyMapping[modelProperty] = labelField;
            }
        }

        public void IndicateBusy(bool force, Action onCursorChanged) {
            var newCursor = Model.IsBusy ? Cursors.Wait : null;
            if (!force && Mouse.OverrideCursor == newCursor) { return; }

            onCursorChanged();
            Mouse.OverrideCursor = newCursor;
        }

        public void OnModelDataChanged(Action onCursorChanged) {
            IndicateBusy(false, onCursorChanged);

            foreach (var modelToWindowPropertyMapping in ModelToWindowPropertyMapping) {
                var modelProperty = modelToWindowPropertyMapping.Key;
                var windowField = modelToWindowPropertyMapping.Value;
                switch (windowField.FieldType.Name) {
                    case "ComboBox":
                        UpdateSelectorIfNecessary((ISelector)modelProperty.GetValue(Model), (WindowsSelector)windowField.GetValue(Window));
                        break;
                    case "ListBox":
                        UpdateSelectorIfNecessary((ISelector)modelProperty.GetValue(Model), (WindowsSelector)windowField.GetValue(Window));
                        break;
                    case "TextBox":
                        UpdateTextBoxIfNecessary((ITextBox)modelProperty.GetValue(Model), (WindowsTextBox)windowField.GetValue(Window));
                        break;
                    case "Button":
                        UpdateButtonIfNecessary((Button)modelProperty.GetValue(Model), (WindowsButton)windowField.GetValue(Window));
                        break;
                    case "WebBrowser":
                        UpdateWebBrowserIfNecessary((IWebBrowser)modelProperty.GetValue(Model), (WindowsWebBrowser)windowField.GetValue(Window));
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            foreach (var modelToWindowLabelPropertyMapping in ModelToWindowLabelPropertyMapping) {
                var modelProperty = modelToWindowLabelPropertyMapping.Key;
                var labelField = modelToWindowLabelPropertyMapping.Value;

                if (labelField.FieldType.Name == "Label") {
                    if (modelProperty.GetValue(Model) is ISelector) {
                        UpdateLabelIfNecessary((ISelector)modelProperty.GetValue(Model), (ContentControl)labelField.GetValue(Window));
                    } else if (modelProperty.GetValue(Model) is ITextBox) {
                        UpdateLabelIfNecessary((ITextBox)modelProperty.GetValue(Model), (ContentControl)labelField.GetValue(Window));
                    } else {
                        throw new NotImplementedException();
                    }
                } else {
                    throw new NotImplementedException();
                }
            }
        }

        private void UpdateLabelIfNecessary(ITextBox modelTextBox, ContentControl label) {
            if (string.IsNullOrWhiteSpace(modelTextBox.LabelText) || label.Content.ToString() == modelTextBox.LabelText) { return; }

            label.Content = modelTextBox.LabelText;
        }

        private void UpdateLabelIfNecessary(ISelector modelSelector, ContentControl label) {
            if (string.IsNullOrWhiteSpace(modelSelector.LabelText) || label.Content.ToString() == modelSelector.LabelText) { return; }

            label.Content = modelSelector.LabelText;
        }

        private void UpdateWebBrowserIfNecessary(IWebBrowser modelWebBrowser, WindowsWebBrowser webBrowser) {
            if (modelWebBrowser.Url == modelWebBrowser.AskedForNavigationToUrl) { return; }

            if (string.IsNullOrWhiteSpace(modelWebBrowser.Url)) {
                modelWebBrowser.AskedForNavigationToUrl = null;
                webBrowser.Navigate((Uri)null);
            } else {
                modelWebBrowser.AskedForNavigationToUrl = modelWebBrowser.Url;
                webBrowser.Navigate(modelWebBrowser.Url);
            }
        }

        private void UpdateButtonIfNecessary(Button modelButton, UIElement windowsButton) {
            var shouldButtonBeEnabled = modelButton.Enabled && !Model.IsBusy;
            if (windowsButton.IsEnabled == shouldButtonBeEnabled) { return; }

            windowsButton.IsEnabled = shouldButtonBeEnabled;
        }

        private void UpdateTextBoxIfNecessary(ITextBox modelTextBox, WindowsTextBox windowsTextBox) {
            Brush brush;
            switch (modelTextBox.Type) {
                case StatusType.Success: brush = Brushes.Green; break;
                case StatusType.Error: brush = Brushes.Red; break;
                default: brush = Brushes.Black; break;
            }

            if (modelTextBox.Text == windowsTextBox.Text && windowsTextBox.Foreground.Equals(brush) && windowsTextBox.IsEnabled == modelTextBox.Enabled) { return; }

            windowsTextBox.IsEnabled = modelTextBox.Enabled;
            windowsTextBox.Text = modelTextBox.Text;
            windowsTextBox.Foreground = brush;
        }

        private void UpdateSelectorIfNecessary(ISelector modelSelector, WindowsSelector windowsSelector) {
            var items = windowsSelector.Items;
            var selectablesChanged = false;
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (items.Count == 0) {
                if (modelSelector.Selectables.Count == 0) {
                    if (windowsSelector.IsEnabled) {
                        windowsSelector.IsEnabled = false;
                    }
                    return;
                }

                windowsSelector.SelectedValuePath = "Guid";
                windowsSelector.DisplayMemberPath = "Name";
                modelSelector.Selectables.ForEach(s => windowsSelector.Items.Add(s));
                selectablesChanged = true;
            } else if (HaveSelectablesChanged(modelSelector.Selectables, windowsSelector.Items)) {
                windowsSelector.Items.Clear();
                modelSelector.Selectables.ForEach(s => windowsSelector.Items.Add(s));
                selectablesChanged = true;
            }

            if (modelSelector.SelectedIndex == windowsSelector.SelectedIndex && !selectablesChanged) { return; }

            windowsSelector.SelectedIndex = modelSelector.SelectedIndex;
            windowsSelector.IsEnabled = modelSelector.Selectables.Any();
        }

        private bool HaveSelectablesChanged(IReadOnlyList<Selectable> selectables, IList itemCollection) {
            if (selectables.Count != itemCollection.Count) { return true; }

            for (var i = 0; i < selectables.Count; i++) {
                if (!(itemCollection[i] is Selectable item)) { return true; }
                if (selectables[i].Guid != item.Guid || selectables[i].Name != item.Name) { return true; }
            }

            return false;
        }
    }
}
