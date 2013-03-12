using System;
using System.Drawing;

namespace MassScheduler.Helpers
{
    public static class ImageHelper
    {
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            if (image == null)
                return null;

            var ratioW = (double) maxWidth / image.Width;
            var ratioH = (double) maxHeight / image.Height;
            var ratio = Math.Min(ratioW, ratioH);

            var scaleW = (int) ( image.Width * ratio );
            var scaleH = (int) ( image.Height * ratio );

            var scaledImage = new Bitmap(scaleW, scaleH);
            Graphics.FromImage(scaledImage).DrawImage(image, 0, 0, scaleW, scaleH);

            return scaledImage;
        }
    }
}