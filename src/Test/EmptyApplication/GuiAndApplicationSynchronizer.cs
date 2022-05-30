using Aspenlaub.Net.GitHub.CSharp.Pegh.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication.GUI;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.EmptyApplication;

public class GuiAndApplicationSynchronizer : GuiAndApplicationSynchronizerBase<ApplicationModel, VishizhukelNetEmptyWindow> {
    public GuiAndApplicationSynchronizer(ApplicationModel model, VishizhukelNetEmptyWindow window, ISimpleLogger simpleLogger, ILogConfigurationFactory logConfigurationFactory)
            : base(model, window, simpleLogger, logConfigurationFactory) {
    }
}