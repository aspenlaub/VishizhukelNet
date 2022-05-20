﻿using System.Windows;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IApplicationModelBase : IBusy {
    ITextBox Status { get; }

    WindowState WindowState { get; set; }

    bool UsesRealBrowserOrView { get; set; }
    IWebView WebView { get; }
    ITextBox WebViewUrl { get; }
    ITextBox WebViewContentSource { get; }
}