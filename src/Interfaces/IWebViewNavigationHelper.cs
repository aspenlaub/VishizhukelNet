﻿using System.Threading.Tasks;
// ReSharper disable UnusedMemberInSuper.Global

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IWebViewNavigationHelper {
    Task<bool> NavigateToUrlAsync(string url);
}