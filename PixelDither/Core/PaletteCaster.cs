using SkiaSharp;

namespace PixelDither.Core
{
    public class PaletteCaster
    {
        public SKBitmap Cast(string filePath, List<SKColor> palette, DistanceMethod distanceMethod)
        {
            var bitmap = SKBitmap.Decode(filePath);
            return Cast(bitmap, palette, distanceMethod);
        }

        public SKBitmap Cast(SKBitmap bitmap, List<SKColor> palette, DistanceMethod distanceMethod)
        {
            ClosestColorFinder closestColorFinder = new ClosestColorFinder();

            var info = bitmap.Info;
            int width = info.Width;
            int height = info.Height;
            int bytesPerPixel = info.BytesPerPixel;
            int rowBytes = info.RowBytes;

            unsafe
            {
                IntPtr pixelsPtr = bitmap.GetPixels();
                Span<byte> pixels = new Span<byte>(pixelsPtr.ToPointer(), height * rowBytes);

                for (int y = 0; y < height; y++)
                {
                    int rowStart = y * rowBytes;

                    for (int x = 0; x < width; x++)
                    {
                        int pixelIndex = rowStart + x * bytesPerPixel;

                        byte blue = pixels[pixelIndex];
                        byte green = pixels[pixelIndex + 1];
                        byte red = pixels[pixelIndex + 2];
                        byte alpha = pixels[pixelIndex + 3];

                        SKColor color = new SKColor(red, green, blue, alpha);

                        SKColor closestColor = closestColorFinder.FindClosestColor(color, palette, distanceMethod);

                        pixels[pixelIndex] = closestColor.Blue;
                        pixels[pixelIndex + 1] = closestColor.Green;
                        pixels[pixelIndex + 2] = closestColor.Red;
                        pixels[pixelIndex + 3] = closestColor.Alpha;
                    }
                }
            } 

            return bitmap;
        }

    }
}
