using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Enums;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers {
    public abstract class TashVerifyAndSetHandlerBase<TModel> : ITashVerifyAndSetHandler<TModel> where TModel : IApplicationModel {
        protected readonly ISimpleLogger SimpleLogger;
        protected readonly ITashSelectorHandler<TModel> TashSelectorHandler;
        protected readonly ITashCommunicator<TModel> TashCommunicator;
        protected readonly Dictionary<string, ISelector> Selectors;

        protected TashVerifyAndSetHandlerBase(ISimpleLogger simpleLogger, ITashSelectorHandler<TModel> tashSelectorHandler, ITashCommunicator<TModel> tashCommunicator,
            Dictionary<string, ISelector> selectors) {
            SimpleLogger = simpleLogger;
            TashSelectorHandler = tashSelectorHandler;
            TashCommunicator = tashCommunicator;
            Selectors = selectors;
        }

        protected abstract Dictionary<string, ITextBox> TextBoxNamesToTextBoxDictionary(ITashTaskHandlingStatus<TModel> status);
        protected abstract Dictionary<string, ISimpleTextHandler> TextBoxNamesToTextHandlerDictionary(ITashTaskHandlingStatus<TModel> status);

        protected virtual void OnValueTaskProcessed(ITashTaskHandlingStatus<TModel> status, bool verify, bool set, string actualValue) {
            if (!verify || actualValue == status.TaskBeingProcessed.Text) {
                SimpleLogger.LogInformation($"{status.TaskBeingProcessed.ControlName} as set to {actualValue}");
            } else {
                SimpleLogger.LogInformation(set
                    ? $"Could not set {status.TaskBeingProcessed.ControlName} to \"{status.TaskBeingProcessed.Text}\", it is \"{actualValue}\""
                    : $"Expected {status.TaskBeingProcessed.ControlName} to be \"{status.TaskBeingProcessed.Text}\", got \"{actualValue}\""
                );
            }
        }

        public async Task ProcessVerifyGetOrSetValueOrLabelTaskAsync(ITashTaskHandlingStatus<TModel> status, bool verify, bool set, bool label, bool combined) {
            string actualValue;
            if (Selectors.ContainsKey(status.TaskBeingProcessed.ControlName)) {
                if (set) {
                    if (combined) {
                        var errorMessage = $"Cannot set items for {status.TaskBeingProcessed.ControlName}, that has not been implemented";
                        SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage}");
                        await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                        return;
                    }
                    SimpleLogger.LogInformation($"Setting value for {status.TaskBeingProcessed.ControlName} via SelectComboOrResetTask");
                    await TashSelectorHandler.ProcessSelectComboOrResetTaskAsync(status);
                    if (status.TaskBeingProcessed.Status != ControllableProcessTaskStatus.Completed) {
                        return;
                    }
                    SimpleLogger.LogInformation($"Value for {status.TaskBeingProcessed.ControlName} set (via SelectComboOrResetTask)");
                }

                var selector = Selectors[status.TaskBeingProcessed.ControlName];
                actualValue = combined
                ? string.Join('^', selector.Selectables.Select(s => s.Name))
                : label ? selector.LabelText : selector.SelectedItem.Name;
            } else if (combined) {
                var errorMessage = $"{status.TaskBeingProcessed.ControlName} is not a selector control";
                SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage}");
                await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                return;
            } else {
                var textBoxes = TextBoxNamesToTextBoxDictionary(status);
                if (!textBoxes.ContainsKey(status.TaskBeingProcessed.ControlName)) {
                    var errorMessage = $"Unknown text or selector control {status.TaskBeingProcessed.ControlName}";
                    SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage}");
                    await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                    return;
                }

                var textHandlers = TextBoxNamesToTextHandlerDictionary(status);
                var textHandler = textHandlers.ContainsKey(status.TaskBeingProcessed.ControlName) ? textHandlers[status.TaskBeingProcessed.ControlName] : null;

                actualValue = await GetOrSetTextBoxValueAsync(textBoxes[status.TaskBeingProcessed.ControlName], textHandler, label, set, status.TaskBeingProcessed.Text);
            }
            OnValueTaskProcessed(status, verify, set, actualValue);
            await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, true, actualValue);
        }

        protected async Task<string> GetOrSetTextBoxValueAsync(ITextBox textBox, ISimpleTextHandler textHandler, bool label, bool set, string text) {
            if (!set) {
                return label ? textBox.LabelText : textBox.Text;
            }

            if (textHandler == null) {
                textBox.Text = text;
            } else {
                await textHandler.TextChangedAsync(text);
            }
            return label ? textBox.LabelText : textBox.Text;
        }

        public async Task ProcessVerifyWhetherEnabledTaskAsync(ITashTaskHandlingStatus<TModel> status) {
            bool actualEnabled;
            var properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var property = properties.FirstOrDefault(p => p.Name == status.TaskBeingProcessed.ControlName);
            if (property == null) {
                var errorMessage = $"Unknown enabled/disabled control {status.TaskBeingProcessed.ControlName}";
                SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
                await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                return;
            }

            var propertyValue = property.GetValue(status.Model);
            switch (propertyValue) {
                case Button button:
                    actualEnabled = button.Enabled;
                    break;
                case ISelector selector:
                    actualEnabled = selector.Selectables.Any();
                    break;
                case TextBox textBox:
                    actualEnabled = textBox.Enabled;
                    break;
                case ToggleButton button:
                    actualEnabled = button.Enabled;
                    break;
                default: {
                    var errorMessage = $"Unknown enabled/disabled control {status.TaskBeingProcessed.ControlName}";
                    SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
                    await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                    return;
                }
            }

            if (status.TaskBeingProcessed.Text == "true") {
                if (actualEnabled) {
                    status.Model.Status.Type = StatusType.Success;
                    status.Model.Status.Text = "";
                } else {
                    status.Model.Status.Type = StatusType.Error;
                    status.Model.Status.Text = $"Expected {status.TaskBeingProcessed.ControlName} to be enabled";
                }
            } else if (actualEnabled) {
                status.Model.Status.Type = StatusType.Error;
                status.Model.Status.Text = $"Expected {status.TaskBeingProcessed.ControlName} to be disabled";
            } else {
                status.Model.Status.Type = StatusType.Success;
                status.Model.Status.Text = "";
            }
            await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
        }

        public async Task ProcessVerifyNumberOfItemsTaskAsync(ITashTaskHandlingStatus<TModel> status) {
            if (string.IsNullOrWhiteSpace(status.TaskBeingProcessed.Text)) {
                var errorMessage = $"No number of items specified for {status.TaskBeingProcessed.ControlName}";
                SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process {errorMessage}");
                await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                return;
            }

            if (!Selectors.ContainsKey(status.TaskBeingProcessed.ControlName)) {
                var errorMessage = $"Unknown control {status.TaskBeingProcessed.ControlName} with number of items";
                SimpleLogger.LogInformation($"Communicating 'BadRequest' to remote controlling process ({errorMessage})");
                await TashCommunicator.ChangeCommunicateAndShowProcessTaskStatusAsync(status, ControllableProcessTaskStatus.BadRequest, false, "", errorMessage);
                return;
            }
            var actualNumberOfItems = Selectors[status.TaskBeingProcessed.ControlName].Selectables.Count;
            status.Model.Status.Text = actualNumberOfItems.ToString() == status.TaskBeingProcessed.Text
                ? ""
                : $"Expected {status.TaskBeingProcessed.Text} item/-s on {status.TaskBeingProcessed.ControlName}, got {actualNumberOfItems}";
            status.Model.Status.Type = string.IsNullOrEmpty(status.Model.Status.Text) ? StatusType.Success : StatusType.Error;
            await TashCommunicator.CommunicateAndShowCompletedOrFailedAsync(status, false, "");
        }
    }
}