using PixelDither.Core;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PixelDither.DitheringAlgorithm.ErrorDiffusion
{
    public enum QuantizationMethod
    {
        Auto,
        Palette
    };

    public class ErrorDiffusion : RenderBase
    {
        public override string FilePath { get; set; }
        public override SKBitmap Bitmap { get; set; }
        public override string NewFilePath { get; set; }
        public static DistanceMethod DistanceMethod { get; set; }
        public static QuantizationMethod QuantizationMethod { get; set; }
        private static int Factor { get; set; }
        private static List<SKColor> Palette { get; set; }
        private static PixelMask[] Algorithm { get; set; }

        public ErrorDiffusion(
            SKBitmap bitmap,
            string newFilePath,
            DistanceMethod distanceMethod,
            QuantizationMethod quantizationMethod,
            PixelMask[] algorithm,
            int factor = 0,
            List<SKColor> palette = null)
        {
            Bitmap = bitmap;
            NewFilePath = newFilePath;
            DistanceMethod = distanceMethod;
            QuantizationMethod = quantizationMethod;
            Factor = factor;
            Algorithm = algorithm;
            Palette = palette;
        }

        public ErrorDiffusion(
            string filePath,
            string newFilePath,
            DistanceMethod distanceMethod,
            QuantizationMethod quantizationMethod,
            PixelMask[] algorithm,
            int factor = 0,
            List<SKColor> palette = null) : this(SKBitmap.Decode(filePath), newFilePath, distanceMethod, quantizationMethod, algorithm, factor, palette)
        {
            FilePath = filePath;
        }

        public override void Render()
        {
            if (Bitmap.ColorType != SKColorType.Bgra8888)
                throw new InvalidOperationException("Bitmap must be in Bgra8888 format.");

            IntPtr pixelsPtr = Bitmap.GetPixels();
            if (pixelsPtr == IntPtr.Zero)
                throw new InvalidOperationException("Failed to access pixel buffer.");

            int width = Bitmap.Width;
            int height = Bitmap.Height;
            int totalPixels = width * height;

            var closestColorFinder = new ClosestColorFinder();

            unsafe
            {
                Span<SKColor> pixelSpan = new Span<SKColor>(pixelsPtr.ToPointer(), totalPixels);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = y * width + x;

                        SKColor originalPixel = pixelSpan[index];

                        SKColor newPixel = QuantizationMethod switch
                        {
                            QuantizationMethod.Auto => closestColorFinder.FindClosestColor(originalPixel, Factor),
                            QuantizationMethod.Palette => closestColorFinder.FindClosestColor(originalPixel, Palette, DistanceMethod),
                            _ => throw new InvalidOperationException("Unknown quantization method")
                        };

                        pixelSpan[index] = newPixel;

                        List<double> quantizationError = [originalPixel.Red - newPixel.Red,
                                                          originalPixel.Green - newPixel.Green,
                                                          originalPixel.Blue - newPixel.Blue];

                        ApplyErrorDiffusion(pixelSpan, Algorithm, x, y, width, quantizationError);
                    }
                }
            }

            Bitmap.NotifyPixelsChanged();
        }

        private void ApplyErrorDiffusion(Span<SKColor> pixelSpan, PixelMask[] pixelMask, int x, int y, int width, List<double> error)
        {
            for (int i = 0; i < pixelMask.Length; i++)
            {
                AdjustPixel(pixelSpan, pixelMask[i], x, y, width, error);
            }
        }

        private void AdjustPixel(Span<SKColor> pixelSpan, PixelMask pixelMask, int x, int y, int width, List<double> error)
        {
            int newX = x + pixelMask.OffsetX;
            int newY = y + pixelMask.OffsetY;
            int index = newY * width + newX;

            if (newX >= 0 && newX < Bitmap.Width && newY >= 0 && newY < Bitmap.Height)
            {
                SKColor pixel = pixelSpan[index];
                SKColor adjustedPixel = new SKColor(
                    AdjustChannel(pixel.Red, error[0], pixelMask.Factor),
                    AdjustChannel(pixel.Green, error[1], pixelMask.Factor),
                    AdjustChannel(pixel.Blue, error[2], pixelMask.Factor),
                    pixel.Alpha);

                pixelSpan[index] = adjustedPixel;
            }
        }

        private byte AdjustChannel(byte original, double error, double factor)
        {
            return (byte)Math.Clamp(Math.Round(original + error * factor), 0, byte.MaxValue);
        }
    }
}
