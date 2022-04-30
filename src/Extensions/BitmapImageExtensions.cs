using System;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Aspenlaub.Net.GitHub.CSharp.VishizhukelNet.Extensions;

public static class BitmapImageExtensions {
    public static bool IsEqualTo(this BitmapImage imageOne, BitmapImage imageTwo) {
        if (imageOne == null || imageTwo == null) {
            return imageOne == null && imageTwo == null;
        }
        return imageOne.ToBytes().SequenceEqual(imageTwo.ToBytes());
    }

    public static byte[] ToBytes(this BitmapImage image) {
        byte[] data = { };
        if (image == null) { return data; }

        try {
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using var stream = new MemoryStream();
            encoder.Save(stream);
            data = stream.ToArray();
            return data;
        } catch (Exception) {
            // ignored
        }

        return data;
    }
}