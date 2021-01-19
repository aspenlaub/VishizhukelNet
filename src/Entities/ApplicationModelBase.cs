using System.Windows;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities {
    // ReSharper disable once UnusedMember.Global
    public class ApplicationModelBase : IApplicationModel {
        public bool IsBusy { get; set; }

        public WindowState WindowState { get; set; }

        public virtual bool IsModelErroneous(out string errorMessage) {
            errorMessage = "";
            return false;
        }
    }
}
