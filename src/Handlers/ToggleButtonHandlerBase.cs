using System.Threading.Tasks;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Extensions;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers {
    public abstract class ToggleButtonHandlerBase<TModel> : IToggleButtonHandler where TModel : IApplicationModel {
        protected readonly TModel Model;
        protected readonly ToggleButton ToggleButton;

        protected ToggleButtonHandlerBase(TModel model, ToggleButton toggleButton) {
            Model = model;
            ToggleButton = toggleButton;
        }

        public bool Unchanged(bool isChecked) {
            return ToggleButton.IsChecked == isChecked;
        }

        public bool IsChecked() {
            return ToggleButton.IsChecked;
        }

        public abstract Task ToggledAsync(bool isChecked);

        public void SetChecked(bool isChecked) {
            ToggleButton.IsChecked = isChecked;
            Model.OtherToggleButtonsWithSameGroup(!isChecked, ToggleButton).ForEach(b => b.IsChecked = false);
        }
    }
}
