using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Enums;
using Aspenlaub.Net.GitHub.CSharp.TashClient.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;
using Aspenlaub.Net.GitHub.CSharp.VishnetIntegrationTestTools;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Integration.Test;

public class WindowUnderTestActions : WindowUnderTestActionsBase {
    public WindowUnderTestActions(ITashAccessor tashAccessor) : base(tashAccessor, "Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test") {
    }

    public ControllableProcessTask CreateSelectBetaTask(ControllableProcess process, string beta) {
        return CreateControllableProcessTask(process, ControllableProcessTaskType.SelectComboItem, nameof(IApplicationModel.Beta), beta);
    }
}