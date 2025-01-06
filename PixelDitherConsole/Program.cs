using PixelDither.Core;
using PixelDither.DitheringAlgorithm;
using PixelDither.DitheringAlgorithm.ErrorDiffusion;
using SkiaSharp;

namespace PixelDitherConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Palette list:");

            var paletteFiles = Directory.EnumerateFiles("palettes").ToArray();

            for (int i = 0; i < paletteFiles.Length; i++)
            {
                Console.WriteLine($"{i}. {Path.GetFileName(paletteFiles[i])}");
            }

            Console.Write("Choose palette -> ");
            if (!int.TryParse(Console.ReadLine(), out var number) || number < 0 || number >= paletteFiles.Length)
            {
                Console.WriteLine("Invalid selection. Exiting.");
                return;
            }

            var palette = GeneratePalette(paletteFiles[number]);

            Console.WriteLine($"[{DateTime.Now}][output.png] Rendering Floyd-Steinberg (7, 3, 5, 1)");

            var algorithm = new DitheringAlgorithmBase().FalseFloydSteinberg();

            var dither = new ErrorDiffusion(
                "input.png",
                "dithered.png",
                DistanceMethod.Euclidean,
                PixelDither.DitheringAlgorithm.ErrorDiffusion.QuantizationMethod.Palette,
                algorithm,
                palette: palette
            );

            dither.Render();
            dither.Save();
        }

        private static List<SKColor> GeneratePalette(string filePath)
        {
            var paletteLines = File.ReadAllLines(filePath);
            var skPalette = new List<SKColor>();

            foreach (var line in paletteLines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    skPalette.Add(SKColor.Parse(line.Trim()));
                }
            }

            var bitmap = new SKBitmap(64 * skPalette.Count, 64);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paint = new SKPaint();

            for (int i = 0; i < skPalette.Count; i++)
            {
                paint.Color = skPalette[i];
                canvas.DrawRect(64 * i, 0, 64, 64, paint);
            }

            var encoded = bitmap.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = new FileStream("palette_preview.png", FileMode.Create, FileAccess.Write);
            encoded.SaveTo(stream);

            Console.WriteLine("Palette preview saved as 'palette_preview.png'.");

            return skPalette;
        }
    }
}
