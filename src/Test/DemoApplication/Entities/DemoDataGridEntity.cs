using System;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Test.DemoApplication.Entities {
    public class DemoDataGridEntity : IDemoDataGridEntity {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
    }
}
