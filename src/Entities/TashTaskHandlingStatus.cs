using System;
using System.Collections.Generic;
using Aspenlaub.Net.GitHub.CSharp.Tash;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Entities {
    public class TashTaskHandlingStatus<TModel> : ITashTaskHandlingStatus<TModel> where TModel : IApplicationModel {
        public TModel Model { get; }
        public int ProcessId { get; }
        public DateTime StatusLastConfirmedAt { get; set; }
        public List<ControllableProcessTask> ControllableProcessTasks { get; set; }
        public ControllableProcessTask TaskBeingProcessed { get; set; }

        public TashTaskHandlingStatus(TModel model, int processId) {
            Model = model;
            ProcessId = processId;
            StatusLastConfirmedAt = DateTime.MinValue;
            ControllableProcessTasks = new List<ControllableProcessTask>();
            TaskBeingProcessed = null;
        }
    }
}
