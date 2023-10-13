using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Entities;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Extensions;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Button = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls.Button;
using ToggleButton = Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls.ToggleButton;
using WindowsTextBox = System.Windows.Controls.TextBox;
using WindowsButton = System.Windows.Controls.Button;
using WindowsSelector = System.Windows.Controls.Primitives.Selector;
using WindowsImage = System.Windows.Controls.Image;
using WindowsToggleButton = System.Windows.Controls.Primitives.ToggleButton;
using WindowsRectangle = System.Windows.Shapes.Rectangle;
using WindowsCollectionViewSource = System.Windows.Data.CollectionViewSource;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;

public abstract class GuiAndApplicationSynchronizerBase<TModel, TWindow, TCollectionViewSourceEntity>
        : IGuiAndApplicationSynchronizer<TModel>
            where TModel : class, IApplicationModelBase
            where TCollectionViewSourceEntity : ICollectionViewSourceEntity {
    protected readonly TWindow Window;
    protected readonly Dictionary<PropertyInfo, FieldInfo> ModelPropertyToWindowFieldMapping, ModelPropertyToWindowLabelMapping;
    protected readonly Dictionary<PropertyInfo, PropertyInfo> ModelPropertyToWindowPropertyMapping;
    protected readonly Dictionary<PropertyInfo, WindowsCollectionViewSource> ModelPropertyToCollectionViewSourceMapping;
    protected readonly ISimpleLogger SimpleLogger;

    public TModel Model { get; }

    protected GuiAndApplicationSynchronizerBase(TModel model, TWindow window, ISimpleLogger simpleLogger) {
        Model = model;
        Window = window;
        SimpleLogger = simpleLogger;
        ModelPropertyToWindowFieldMapping = new Dictionary<PropertyInfo, FieldInfo>();
        ModelPropertyToWindowLabelMapping = new Dictionary<PropertyInfo, FieldInfo>();
        ModelPropertyToWindowPropertyMapping = new Dictionary<PropertyInfo, PropertyInfo>();
        ModelPropertyToCollectionViewSourceMapping = new Dictionary<PropertyInfo, WindowsCollectionViewSource>();
        var modelProperties = typeof(TModel).GetPropertiesAndInterfaceProperties();
        var windowFields = typeof(TWindow).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        var windowProperties = typeof(TWindow).GetProperties(BindingFlags.Public | BindingFlags.Instance ).Where(p => p.CanRead && p.CanWrite).ToList();
        var windowAsFrameworkElement = window as FrameworkElement;
        foreach (var modelProperty in modelProperties) {
            if (windowAsFrameworkElement?.TryFindResource(modelProperty.Name) is WindowsCollectionViewSource viewSource) {
                ModelPropertyToCollectionViewSourceMapping[modelProperty] = viewSource;
                continue;
            }

            var windowField = windowFields.FirstOrDefault(p => p.Name == modelProperty.Name);
            if (windowField != null) {
                ModelPropertyToWindowFieldMapping[modelProperty] = windowField;
                continue;
            }

            var labelField = windowFields.FirstOrDefault(p => p.Name == modelProperty.Name + "Label");
            if (labelField != null) {
                ModelPropertyToWindowLabelMapping[modelProperty] = labelField;
                continue;
            }

            var windowProperty = windowProperties.FirstOrDefault(p => p.Name == modelProperty.Name);
            if (windowProperty == null) {
                continue;
            }

            ModelPropertyToWindowPropertyMapping[modelProperty] = windowProperty;
        }
    }

    public void IndicateBusy(bool force) {
        var newCursor = Model.IsBusy ? Cursors.Wait : null;
        if (!force && Mouse.OverrideCursor == newCursor) { return; }

        OnCursorChanged();
        Mouse.OverrideCursor = newCursor;
    }

    public virtual void OnCursorChanged() {
    }

    public async Task OnModelDataChangedAsync() {
        using (SimpleLogger.BeginScope(SimpleLoggingScopeId.Create(nameof(OnModelDataChangedAsync) + "Base"))) {
            IndicateBusy(false);

            foreach (var modelPropertyToWindowFieldMapping in ModelPropertyToWindowFieldMapping) {
                var modelProperty = modelPropertyToWindowFieldMapping.Key;
                var windowField = modelPropertyToWindowFieldMapping.Value;
                await UpdateFieldIfNecessaryAsync(windowField, modelProperty);
            }

            foreach (var modelPropertyToWindowLabelMapping in ModelPropertyToWindowLabelMapping) {
                var modelProperty = modelPropertyToWindowLabelMapping.Key;
                var labelField = modelPropertyToWindowLabelMapping.Value;

                if (labelField.FieldType.Name == "Label") {
                    if (modelProperty.GetValue(Model) is ISelector) {
                        UpdateLabelIfNecessary((ISelector)modelProperty.GetValue(Model), (ContentControl)labelField.GetValue(Window));
                    }
                    else if (modelProperty.GetValue(Model) is ITextBox) {
                        UpdateLabelIfNecessary((ITextBox)modelProperty.GetValue(Model), (ContentControl)labelField.GetValue(Window));
                    }
                    else {
                        throw new NotImplementedException($"{nameof(labelField)} is neither a selector nor a text box");
                    }
                }
                else {
                    throw new NotImplementedException($"{labelField.FieldType.Name} is not implemented yet");
                }
            }

            foreach (var modelPropertyToWindowPropertyMapping in ModelPropertyToWindowPropertyMapping) {
                var modelProperty = modelPropertyToWindowPropertyMapping.Key;
                var windowProperty = modelPropertyToWindowPropertyMapping.Value;
                switch (windowProperty.PropertyType.Name) {
                    case "WindowState":
                        // ReSharper disable once PossibleNullReferenceException
                        UpdateWindowStateIfNecessary((WindowState)modelProperty.GetValue(Model), Window as Window);
                        break;
                    default:
                        throw new NotImplementedException($"Window property type {windowProperty.PropertyType.Name} is not implemented yet");
                }
            }

            foreach (var modelPropertyToCollectionViewSourceMapping in ModelPropertyToCollectionViewSourceMapping) {
                var modelProperty = modelPropertyToCollectionViewSourceMapping.Key;
                var collectionViewSource = modelPropertyToCollectionViewSourceMapping.Value;
                UpdateCollectionViewSourceIfNecessary((ICollectionViewSource<TCollectionViewSourceEntity>)modelProperty.GetValue(Model), collectionViewSource);
            }

            await Task.CompletedTask;
        }
    }

    protected virtual async Task UpdateFieldIfNecessaryAsync(FieldInfo windowField, PropertyInfo modelProperty) {
        switch (windowField.FieldType.Name) {
            case "ComboBox":
                UpdateSelectorIfNecessary((ISelector)modelProperty.GetValue(Model),
                    (WindowsSelector)windowField.GetValue(Window));
                break;
            case "ListBox":
                UpdateSelectorIfNecessary((ISelector)modelProperty.GetValue(Model),
                    (WindowsSelector)windowField.GetValue(Window));
                break;
            case "TextBox":
                UpdateTextBoxIfNecessary((ITextBox)modelProperty.GetValue(Model),
                    (WindowsTextBox)windowField.GetValue(Window));
                break;
            case "Button":
                UpdateButtonIfNecessary((Button)modelProperty.GetValue(Model), (WindowsButton)windowField.GetValue(Window));
                break;
            case "Image":
                UpdateImageIfNecessary((IImage)modelProperty.GetValue(Model), (WindowsImage)windowField.GetValue(Window));
                break;
            case "RadioButton":
                UpdateToggleButtonIfNecessary((ToggleButton)modelProperty.GetValue(Model),
                    (WindowsToggleButton)windowField.GetValue(Window));
                break;
            case "Rectangle":
                UpdateRectangleIfNecessary((IRectangle)modelProperty.GetValue(Model),
                    (WindowsRectangle)windowField.GetValue(Window));
                break;
            case "DataGrid":
            case "EnvironmentType":
                await Task.CompletedTask;
                return;
            default:
                throw new NotImplementedException($"Field type name {windowField.FieldType.Name} is not implemented yet.");
        }
    }

    private void UpdateRectangleIfNecessary(IRectangle modelRectangle, Shape rectangle) {
        if ((int)modelRectangle.Left == (int)Canvas.GetLeft(rectangle)
            && (int)modelRectangle.Top == (int)Canvas.GetTop(rectangle)
            && (int)modelRectangle.Width == (int)rectangle.Width
            && (int)modelRectangle.Height == (int)rectangle.Height
            && (int)modelRectangle.StrokeThickness == (int)rectangle.StrokeThickness
            && modelRectangle.Stroke.Equals(rectangle.Stroke)) { return; }

        Canvas.SetLeft(rectangle, modelRectangle.Left);
        Canvas.SetTop(rectangle, modelRectangle.Top);
        rectangle.Width = modelRectangle.Width;
        rectangle.Height = modelRectangle.Height;
        rectangle.Stroke = modelRectangle.Stroke;
        rectangle.StrokeThickness = modelRectangle.StrokeThickness;
    }

    public virtual void OnImageChanged(WindowsImage image) {
    }

    private void UpdateImageIfNecessary(IImage modelImage, WindowsImage image) {
        if (modelImage == null) {
            throw new ArgumentNullException(nameof(modelImage));
        }

        var imageSource = image.Source as BitmapImage;

        if (imageSource.IsEqualTo(modelImage.BitmapImage)) { return; }

        image.Source = modelImage.BitmapImage;

        OnImageChanged(image);
    }

    private void UpdateLabelIfNecessary(ITextBox modelTextBox, ContentControl label) {
        if (modelTextBox == null) {
            throw new ArgumentNullException(nameof(modelTextBox));
        }

        if (string.IsNullOrWhiteSpace(modelTextBox.LabelText) || label.Content.ToString() == modelTextBox.LabelText) { return; }

        label.Content = modelTextBox.LabelText;
    }

    private void UpdateLabelIfNecessary(ISelector modelSelector, ContentControl label) {
        if (modelSelector == null) {
            throw new ArgumentNullException(nameof(modelSelector));
        }

        if (string.IsNullOrWhiteSpace(modelSelector.LabelText) || label.Content.ToString() == modelSelector.LabelText) { return; }

        label.Content = modelSelector.LabelText;
    }

    private void UpdateButtonIfNecessary(Button modelButton, UIElement windowsButton) {
        if (modelButton == null) {
            throw new ArgumentNullException(nameof(modelButton));
        }

        var shouldButtonBeEnabled = modelButton.Enabled && !Model.IsBusy;
        if (windowsButton.IsEnabled == shouldButtonBeEnabled) { return; }

        windowsButton.IsEnabled = shouldButtonBeEnabled;
    }

    private void UpdateTextBoxIfNecessary(ITextBox modelTextBox, WindowsTextBox windowsTextBox) {
        if (modelTextBox == null) {
            throw new ArgumentNullException(nameof(modelTextBox));
        }

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
        if (modelSelector == null) {
            throw new ArgumentNullException(nameof(modelSelector));
        }

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

    private void UpdateToggleButtonIfNecessary(ToggleButton modelButton, WindowsToggleButton windowsToggleButton) {
        var shouldButtonBeEnabled = modelButton.Enabled && !Model.IsBusy;
        if (windowsToggleButton.IsEnabled == shouldButtonBeEnabled && windowsToggleButton.IsChecked == modelButton.IsChecked) { return; }

        windowsToggleButton.IsEnabled = shouldButtonBeEnabled;
        windowsToggleButton.IsChecked = modelButton.IsChecked;
    }

    private void UpdateWindowStateIfNecessary(WindowState modelWindowState, Window window) {
        if (modelWindowState == window.WindowState) { return; }

        window.WindowState = modelWindowState;
    }

    private void UpdateCollectionViewSourceIfNecessary(ICollectionViewSource<TCollectionViewSourceEntity> modelCollectionViewSource, WindowsCollectionViewSource windowCollectionViewSource) {
        if (windowCollectionViewSource == null) { return; }

        var sortedOldItems = windowCollectionViewSource.View?.SourceCollection.Cast<object>().Cast<ICollectionViewSourceEntity>().OrderBy(item => item.Guid).ToList();
        sortedOldItems ??= new List<ICollectionViewSourceEntity>();
        var newItems = new ObservableCollection<ICollectionViewSourceEntity>();
        foreach (var row in modelCollectionViewSource.Items.Where(row => row.GetType() == modelCollectionViewSource.EntityType)) {
            newItems.Add(row);
        }

        var sortedNewItems = newItems.OrderBy(item => item.Guid).ToList();

        if (sortedOldItems.SequenceEqual(sortedNewItems)) {
            return;
        }

        windowCollectionViewSource.Source = newItems;
    }
}