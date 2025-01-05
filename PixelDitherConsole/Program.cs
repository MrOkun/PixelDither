using PixelDither.Core;
using PixelDither.DitheringAlgorithm;
using SkiaSharp;

namespace PixelDitherConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Palette list:");

            var paletteFiles = Directory.EnumerateFiles("palettes").ToArray();

            //var palettes = new List<List<SKColor>>();

            for (int i = 0; i < paletteFiles.Length; i++)
            {
                Console.WriteLine($"{i}. {paletteFiles[i]}");
                //palettes.Add(GeneretaPalette(paletteFiles[i]));
            }

            Console.Write("Choose palette -> ");
            var number = int.Parse(Console.ReadLine());

            var palette = GeneretaPalette(paletteFiles[number]);

            Console.WriteLine($"[{DateTime.Now}][output.png] Rendering Floyd-Steinberd (7, 3, 5, 1)");

            //int[] coefficientsFloydSteinberg = [-4, 4, 12, 4];
            int[] coefficientsFloydSteinberg = [7, 3, 5, 1];
            FloydSteinbergDithering ditherFloydSteinberg = new FloydSteinbergDithering("input.png",
                                                                                       "dithered.png",
                                                                                       DistanceMethod.Euclidean,
                                                                                       QuantizationMethod.Palette,
                                                                                       coefficientsFloydSteinberg,
                                                                                       palette: palette);

            ditherFloydSteinberg.Dither();
            ditherFloydSteinberg.Save();

            PaletteCaster paletteCaster = new PaletteCaster();
            var colored = paletteCaster.Cast(palette, SKBitmap.Decode("dithered.png"), DistanceMethod.Euclidean);

            var encoded = colored.Encode(SKEncodedImageFormat.Png, 8);
            using var stream = new FileStream("colored.png", FileMode.Create, FileAccess.Write);
            encoded.SaveTo(stream);

            /*
            for (int i = 0; i < palettes.Count; i++)
            {
                Console.WriteLine($"[{DateTime.Now}][{i}_{paletteFiles[i]}.png] Rendering Floyd-Steinberd (7, 3, 5, 1)");

                int[] coefficientsFloydSteinberg = [7, 3, 5, 1];
                FloydSteinbergDithering ditherFloydSteinberg = new FloydSteinbergDithering("input.png", $"{i}.png", DistanceMethod.Euclidean, QuantizationMethod.Palette, coefficientsFloydSteinberg, palette: palettes[i]);

                ditherFloydSteinberg.Dither();
                ditherFloydSteinberg.Save();
            }
            */

            /*
            Console.WriteLine($"[{DateTime.Now}][1_output_equal.png] Rendering equal distribution (4, 4, 4, 4)");
            //4, 4, 4, 4 (equal distribution)
            int[] coefficientsEqualDistribution = [4, 4, 4, 4];
            FloydSteinbergDithering ditherEqualDistribution = new FloydSteinbergDithering("input.png", "1_output_equal.png", DistanceMethod.Euclidean, QuantizationMethod.Palette, coefficientsEqualDistribution, palette: SKPalette);

            ditherEqualDistribution.Dither();
            ditherEqualDistribution.Save();

            Console.WriteLine($"[{DateTime.Now}][2_output_rd.png] Rendering equal distribution (8, 0, 8, 0)");
            //8, 0, 8, 0 (right and down only)
            int[] coefficientsRightAndDown = [8, 0, 8, 0];
            FloydSteinbergDithering ditherCoefficientsRightAndDown = new FloydSteinbergDithering("input.png", "2_output_rd.png", DistanceMethod.Euclidean, QuantizationMethod.Palette, coefficientsRightAndDown, palette: SKPalette);

            ditherCoefficientsRightAndDown.Dither();
            ditherCoefficientsRightAndDown.Save();

            Console.WriteLine($"[{DateTime.Now}][2_output_diagonals.png] Rendering equal distribution (0, 8, 0, 8)");
            // 0, 8, 0, 8 (diagonals only)
            int[] coefficientsDiagonals = [0, 8, 0, 8];
            FloydSteinbergDithering ditherCoefficientsDiagonals = new FloydSteinbergDithering("input.png", "2_output_diagonals.png", DistanceMethod.Euclidean, QuantizationMethod.Palette, coefficientsDiagonals, palette: SKPalette);

            ditherCoefficientsDiagonals.Dither();
            ditherCoefficientsDiagonals.Save();

            Console.WriteLine($"[{DateTime.Now}][2_output_rdl.png] Rendering equal distribution (8, 8, 0, 0)");
            //8, 8, 0, 0 (right and down-left only)
            int[] coefficientsRightAndDownLeft = [8, 8, 0, 0];
            FloydSteinbergDithering ditherCoefficientsRightAndDownLeft = new FloydSteinbergDithering("input.png", "2_output_rdl.png", DistanceMethod.Euclidean, QuantizationMethod.Palette, coefficientsRightAndDownLeft, palette: SKPalette);

            ditherCoefficientsRightAndDownLeft.Dither();
            ditherCoefficientsRightAndDownLeft.Save();
            */
        }

        private static List<SKColor> GeneretaPalette(string name)
        {
            var paletteFile = File.ReadAllText(name);

            var palette = paletteFile.Split('\n');

            var SKPalette = new List<SKColor>();
            for (int i = 0; i < palette.Count() - 1; i++)
                SKPalette.Add(SKColor.Parse(palette[i]));

            var bitmap = new SKBitmap(64 * palette.Count() - 1, 64);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            var paint = new SKPaint();

            for (int i = 0; i < palette.Count() - 1; i++)
            {
                paint.Color = SKColor.Parse(palette[i]);

                canvas.DrawRect(64 * i, 0, 64, 64, paint);
            }

            var encoded = bitmap.Encode(SKEncodedImageFormat.Png, 2);
            using var stream = new FileStream("palette.jpg", FileMode.Create, FileAccess.Write);
            encoded.SaveTo(stream);

            return SKPalette;
        }
    }
}