using System.IO;
using System.Windows.Media.Imaging;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Extensions {
    public static class MemoryStreamExtensions {
        public static BitmapImage ToBitmapImage(this MemoryStream stream) {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
    }
}
