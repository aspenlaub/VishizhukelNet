using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebBrowserApplication;

public class GuiAndApplicationSynchronizer : GuiAndApplicationSynchronizerBase<IApplicationModel, VishizhukelNetWebBrowserWindow>, IGuiAndApplicationSynchronizer {
    public GuiAndApplicationSynchronizer(IApplicationModel model, VishizhukelNetWebBrowserWindow window, IApplicationLogger applicationLogger) : base(model, window, applicationLogger) {
    }
}