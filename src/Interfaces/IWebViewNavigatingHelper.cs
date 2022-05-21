﻿using System;
using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IWebViewNavigatingHelper {
    Task<bool> WaitUntilNotNavigatingAnymoreAsync(string url, DateTime minLastUpdateTime);
}