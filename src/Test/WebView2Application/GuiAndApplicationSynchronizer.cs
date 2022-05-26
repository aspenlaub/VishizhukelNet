﻿using Aspenlaub.Net.GitHub.CSharp.Vishizhukel.Interfaces.Application;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.GUI;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.Entities;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application.GUI;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.WebView2Application;

public class GuiAndApplicationSynchronizer : GuiAndWebViewApplicationSynchronizerBase<ApplicationModel, VishizhukelNetWebView2Window> {
    public GuiAndApplicationSynchronizer(ApplicationModel model, VishizhukelNetWebView2Window window, IApplicationLogger applicationLogger) : base(model, window, applicationLogger) {
    }
}