﻿using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface IGuiAndAppHandler {
        Task EnableOrDisableButtonsThenSyncGuiAndAppAsync();
        // ReSharper disable once UnusedMember.Global
        void IndicateBusy(bool force);
    }
}
