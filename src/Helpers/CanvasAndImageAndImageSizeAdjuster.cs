using System.Windows;
using System.Windows.Controls;
using Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Interfaces;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Helpers {
    public class CanvasAndImageAndImageSizeAdjuster : ICanvasAndImageSizeAdjuster {
        public void AdjustCanvasAndImage(FrameworkElement canvasContainer, FrameworkElement canvas, Image image) {
            if (canvasContainer == null || canvas == null || image?.Source == null) { return; }

            if ((int)canvas.Height == (int)canvasContainer.ActualHeight && (int)canvas.Width == (int)canvasContainer.ActualWidth) { return; }

            double canvasWidth, canvasHeight;
            if (image.Source.Height > image.Source.Width) {
                AdjustAssumingLandscape(canvasContainer.ActualHeight, canvasContainer.ActualWidth, image.Source.Height, image.Source.Width,
                    out canvasHeight, out canvasWidth);
            } else {
                AdjustAssumingLandscape(canvasContainer.ActualWidth, canvasContainer.ActualHeight, image.Source.Width, image.Source.Height,
                    out canvasWidth, out canvasHeight);
            }

            canvas.Width = canvasWidth;
            canvas.Height = canvasHeight;
            image.Width = canvas.Width;
            image.Height = canvas.Height;
        }

        private static void AdjustAssumingLandscape(double actualContainerWidth, double actualContainerHeight, double pictureSourceWidth, double pictureSourceHeight,
                out double canvasWidth, out double canvasHeight) {
            var containerRatio = actualContainerWidth / actualContainerHeight;
            var pictureRatio = pictureSourceWidth / pictureSourceHeight;

            if (containerRatio <= pictureRatio) {
                canvasWidth = actualContainerWidth;
                canvasHeight = actualContainerWidth / pictureRatio;
            } else {
                canvasHeight = actualContainerHeight;
                canvasWidth = actualContainerHeight * pictureRatio;
            }
        }
    }
}
