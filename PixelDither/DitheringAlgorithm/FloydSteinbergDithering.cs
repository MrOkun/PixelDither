using PixelDither.Core;
using SkiaSharp;

namespace PixelDither.DitheringAlgorithm
{
    public enum QuantizationMethod
    {
        Auto,
        Palette
    };

    public class FloydSteinbergDithering : Dithering
    {
        public override string FilePath { get; set; }
        public override SKBitmap Bitmap { get; set; }
        public override string NewFilePath { get; set; }
        public static DistanceMethod DistanceMethod { get; set; }
        public static QuantizationMethod QuantizationMethod { get; set; }
        private static int Factor { get; set; }
        private static List<SKColor> Palette { get; set; }
        private static int[] Coefficients { get; set; }

        public FloydSteinbergDithering(
            SKBitmap bitmap,
            string newFilePath,
            DistanceMethod distanceMethod,
            QuantizationMethod quantizationMethod,
            int[] coefficients,
            int factor = 0,
            List<SKColor> palette = null)
        {
            Bitmap = bitmap;
            NewFilePath = newFilePath;
            DistanceMethod = distanceMethod;
            QuantizationMethod = quantizationMethod;
            Factor = factor;
            Coefficients = coefficients;
            Palette = palette;
        }

        public FloydSteinbergDithering(
            string filePath,
            string newFilePath,
            DistanceMethod distanceMethod,
            QuantizationMethod quantizationMethod,
            int[] coefficients,
            int factor = 0,
            List<SKColor> palette = null) : this(SKBitmap.Decode(filePath), newFilePath, distanceMethod, quantizationMethod, coefficients, factor, palette)
        {
            FilePath = filePath;
        }

        public override void Dither()
        {
            for (int i = 0; i < Bitmap.Width; i++)
            {
                for (int j = 0; j < Bitmap.Height; j++)
                {
                    SKColor originalPixel = Bitmap.GetPixel(i, j);
                    SKColor newPixel = QuantizationMethod switch
                    {
                        QuantizationMethod.Auto => new ClosestColorFinder().FindClosestColor(originalPixel, Factor),
                        QuantizationMethod.Palette => new ClosestColorFinder().FindClosestColor(originalPixel, Palette, DistanceMethod),
                        _ => throw new InvalidOperationException("Unknown quantization method")
                    };

                    Bitmap.SetPixel(i, j, newPixel);

                    var quantizationError = new
                    {
                        Red = originalPixel.Red - newPixel.Red,
                        Green = originalPixel.Green - newPixel.Green,
                        Blue = originalPixel.Blue - newPixel.Blue
                    };

                    ApplyErrorDiffusion(i, j, quantizationError);
                }
            }
        }

        private void ApplyErrorDiffusion(int x, int y, dynamic error)
        {
            void AdjustPixel(int offsetX, int offsetY, double factor)
            {
                int newX = x + offsetX;
                int newY = y + offsetY;

                if (newX >= 0 && newX < Bitmap.Width && newY >= 0 && newY < Bitmap.Height)
                {
                    var pixel = Bitmap.GetPixel(newX, newY);
                    var adjustedPixel = new SKColor(
                        AdjustChannel(pixel.Red, error.Red, factor),
                        AdjustChannel(pixel.Green, error.Green, factor),
                        AdjustChannel(pixel.Blue, error.Blue, factor));
                    Bitmap.SetPixel(newX, newY, adjustedPixel);
                }
            }

            AdjustPixel(1, 0, Coefficients[0] / 16.0);
            AdjustPixel(-1, 1, Coefficients[1] / 16.0);
            AdjustPixel(0, 1, Coefficients[2] / 16.0);
            AdjustPixel(1, 1, Coefficients[3] / 16.0);
        }

        private byte AdjustChannel(byte original, int error, double factor)
        {
            return (byte)Math.Clamp(Math.Round(original + error * factor), 0, byte.MaxValue);
        }

        private static SKColor FindClosestColor(SKColor originalColor, int factor)
        {
            byte red = (byte)Math.Clamp(Math.Round((double)(originalColor.Red / factor), 0) * factor, 0, byte.MaxValue);
            byte green = (byte)Math.Clamp(Math.Round((double)(originalColor.Green / factor), 0) * factor, 0, byte.MaxValue);
            byte blue = (byte)Math.Clamp(Math.Round((double)(originalColor.Blue / factor), 0) * factor, 0, byte.MaxValue);

            return new SKColor(red, green, blue);
        }

        private static SKColor FindClosestColor(SKColor originalColor, List<SKColor> palette)
        {
            SKColor nearestColor = SKColor.Empty;
            double nearestColorDistance = double.MaxValue;

            for (int i = 0; i < palette.Count; i++)
            {
                double distance = double.MaxValue;

                switch (DistanceMethod)
                {
                    case DistanceMethod.Euclidean:
                        distance = Math.Sqrt(Math.Pow(originalColor.Red - palette[i].Red, 2) +
                                             Math.Pow(originalColor.Green - palette[i].Green, 2) +
                                             Math.Pow(originalColor.Blue - palette[i].Blue, 2));
                        break;
                    case DistanceMethod.Manhattan:
                        distance = Math.Abs(originalColor.Red - palette[i].Red)
                                 + Math.Abs(originalColor.Green - palette[i].Green)
                                 + Math.Abs(originalColor.Blue - palette[i].Blue);
                        break;
                    case DistanceMethod.DeltaRGB:
                        var red = Math.Abs(originalColor.Red - palette[i].Red);
                        var green = Math.Abs(originalColor.Green - palette[i].Green);
                        var blue = Math.Abs(originalColor.Blue - palette[i].Blue);

                        distance = Math.Max(red, Math.Max(green, blue));
                        break;
                }

                if (distance < nearestColorDistance)
                {
                    nearestColorDistance = distance;
                    nearestColor = palette[i];
                }
            }

            return nearestColor;
        }
    }
}
