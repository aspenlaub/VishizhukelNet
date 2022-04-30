using System.Windows.Media;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;

public class Rectangle : IRectangle {
    public double Left { get; set; } = 0;
    public double Top { get; set; } = 0;
    public double Width { get; set; } = 0;
    public double Height { get; set; } = 0;
    public Brush Stroke { get; set; } = new SolidColorBrush(Colors.Black);
    public double StrokeThickness { get; set; } = 1;
}