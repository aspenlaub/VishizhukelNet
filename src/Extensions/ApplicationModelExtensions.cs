using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Extensions {
    public static class ApplicationModelExtensions {
        public static List<ToggleButton> OtherToggleButtonsWithSameGroup(this IApplicationModel model, bool returnEmptyList, ToggleButton toggleButton) {
            var groupName = toggleButton.GroupName;
            var result = new List<ToggleButton>();
            if (returnEmptyList || groupName == "") { return result; }

            foreach (var property in ToggleButtonProperties(model.GetType())) {
                var otherToggleButton = property.GetValue(model) as ToggleButton;
                if (otherToggleButton == null || otherToggleButton == toggleButton) { continue; }

                result.Add(otherToggleButton);
            }
            return result;
        }

        private static List<PropertyInfo> ToggleButtonProperties(Type t) {
            return t.GetProperties().Where(p =>
                p.PropertyType == typeof(ToggleButton)
            ).ToList();
        }
    }
}
