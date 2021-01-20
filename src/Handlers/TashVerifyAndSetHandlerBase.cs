using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.Tash;
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

        public abstract Task ProcessVerifyWhetherEnabledTaskAsync(ITashTaskHandlingStatus<TModel> status);
        public abstract Task ProcessVerifyNumberOfItemsTaskAsync(ITashTaskHandlingStatus<TModel> status);
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
    }
}
