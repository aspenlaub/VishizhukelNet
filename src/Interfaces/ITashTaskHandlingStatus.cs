using System;
using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.Tash;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces {
    public interface ITashTaskHandlingStatus<out TModel> where TModel : IApplicationModel {
        TModel Model { get; }
        int ProcessId { get; }
        DateTime StatusLastConfirmedAt { get; set; }
        List<ControllableProcessTask> ControllableProcessTasks { get; set; }
        ControllableProcessTask TaskBeingProcessed { get; set; }
    }
}