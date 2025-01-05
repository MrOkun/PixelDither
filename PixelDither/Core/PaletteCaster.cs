using SkiaSharp;

namespace PixelDither.Core
{
    public class PaletteCaster
    {
        public SKBitmap Cast(List<SKColor> palette, SKBitmap bitmap, DistanceMethod distanceMethod)
        {
            ClosestColorFinder closestColorFinder = new ClosestColorFinder();

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);

                    var closestColor = closestColorFinder.FindClosestColor(pixel, palette, distanceMethod);

                    bitmap.SetPixel(i, j, closestColor);
                }
            }

            return bitmap;
        }
    }
}
