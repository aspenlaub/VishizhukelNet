using System.Windows.Media;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

public interface IRectangle {
    double Left { get; set; }
    double Top { get; set; }
    double Width { get; set; }
    double Height { get; set; }
    Brush Stroke { get; set; }
    double StrokeThickness { get; set; }
}