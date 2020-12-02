using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Extensions;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Handlers {
    public abstract class ToggleButtonHandlerBase<TModel> where TModel : IApplicationModel {
        protected readonly TModel Model;
        protected readonly ToggleButton ToggleButton;

        protected ToggleButtonHandlerBase(TModel model, ToggleButton toggleButton) {
            Model = model;
            ToggleButton = toggleButton;
        }

        protected bool Unchanged(bool isChecked) {
            return ToggleButton.IsChecked == isChecked;
        }

        protected void SetChecked(bool isChecked) {
            ToggleButton.IsChecked = isChecked;
            Model.OtherToggleButtonsWithSameGroup(!isChecked, ToggleButton).ForEach(b => b.IsChecked = false);
        }
    }
}
