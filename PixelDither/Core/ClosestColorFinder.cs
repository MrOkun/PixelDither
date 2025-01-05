using SkiaSharp;

namespace PixelDither.Core
{
    public enum DistanceMethod
    {
        Euclidean,
        Manhattan,
        DeltaRGB
    };

    public class ClosestColorFinder
    {
        public SKColor FindClosestColor(SKColor originalColor, List<SKColor> palette, DistanceMethod distanceMethod)
        {
            SKColor nearestColor = SKColor.Empty;
            double nearestColorDistance = double.MaxValue;

            for (int i = 0; i < palette.Count; i++)
            {
                double distance = double.MaxValue;

                switch (distanceMethod)
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

        public SKColor FindClosestColor(SKColor originalColor, int factor)
        {
            byte red = (byte)Math.Clamp(Math.Round((double)(originalColor.Red / factor), 0) * factor, 0, byte.MaxValue);
            byte green = (byte)Math.Clamp(Math.Round((double)(originalColor.Green / factor), 0) * factor, 0, byte.MaxValue);
            byte blue = (byte)Math.Clamp(Math.Round((double)(originalColor.Blue / factor), 0) * factor, 0, byte.MaxValue);

            return new SKColor(red, green, blue);
        }
    }
}
