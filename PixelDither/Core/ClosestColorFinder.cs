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

            foreach (var color in palette)
            {
                double distance = distanceMethod switch
                {
                    DistanceMethod.Euclidean => CalculateEuclideanDistance(originalColor, color),
                    DistanceMethod.Manhattan => CalculateManhattanDistance(originalColor, color),
                    DistanceMethod.DeltaRGB => CalculateDeltaRGBDistance(originalColor, color),
                    _ => throw new ArgumentOutOfRangeException(nameof(distanceMethod), "Unsupported distance method")
                };

                if (distance < nearestColorDistance)
                {
                    nearestColorDistance = distance;
                    nearestColor = color;
                }
            }

            return nearestColor;
        }

        public SKColor FindClosestColor(SKColor originalColor, int factor)
        {
            byte red = QuantizeChannel(originalColor.Red, factor);
            byte green = QuantizeChannel(originalColor.Green, factor);
            byte blue = QuantizeChannel(originalColor.Blue, factor);

            return new SKColor(red, green, blue);
        }

        public double CalculateEuclideanDistance(SKColor color1, SKColor color2)
        {
            return Math.Sqrt(
                Math.Pow(color1.Red - color2.Red, 2) +
                Math.Pow(color1.Green - color2.Green, 2) +
                Math.Pow(color1.Blue - color2.Blue, 2));
        }

        public double CalculateManhattanDistance(SKColor color1, SKColor color2)
        {
            return Math.Abs(color1.Red - color2.Red) +
                   Math.Abs(color1.Green - color2.Green) +
                   Math.Abs(color1.Blue - color2.Blue);
        }

        public double CalculateDeltaRGBDistance(SKColor color1, SKColor color2)
        {
            return Math.Max(
                Math.Abs(color1.Red - color2.Red),
                Math.Max(
                    Math.Abs(color1.Green - color2.Green),
                    Math.Abs(color1.Blue - color2.Blue)));
        }

        public byte QuantizeChannel(byte channel, int factor)
        {
            return (byte)Math.Clamp(Math.Round(channel / (double)factor) * factor, 0, byte.MaxValue);
        }
    }
}
