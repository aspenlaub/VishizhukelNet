using System.Windows.Media.Imaging;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Controls;

public class Image : IImage {
    public BitmapImage BitmapImage { get; set; } = new();
}