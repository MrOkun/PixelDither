using SkiaSharp;

namespace PixelDither.DitheringAlgorithm
{
    public enum RandomDitherType
    {
        Colored,
        BlackAndWhite
    }

    public class RandomDithering : Dithering
    {
        public override string FilePath { get; set; }
        public override SKBitmap Bitmap { get; set; }
        public override string NewFilePath { get; set; }
        public RandomDitherType RandomDitherType { get; set; }
        public byte Threshold { get; set; }

        public RandomDithering(string filePath, string newFilePath, RandomDitherType randomDitherType, byte threshold)
        {
            FilePath = filePath;
            NewFilePath = newFilePath;
            RandomDitherType = randomDitherType;
            Threshold = threshold;

            Bitmap = SKBitmap.Decode(FilePath);
        }

        public override void Dither()
        {
            for (int i = 0; i < Bitmap.Width; i++)
            {
                for (int j = 0; j < Bitmap.Height; j++)
                {
                    var pixel = Bitmap.GetPixel(i, j);
                    byte pixelColor = (byte)Math.Round((0.299f * pixel.Red) + (0.587f * pixel.Green) + (0.114 * pixel.Blue), 0);

                    SKColor newPixel = new SKColor();

                    var randomValue = new Random().Next(-125, 125);

                    if (RandomDitherType == RandomDitherType.BlackAndWhite)
                    {
                        if (pixelColor > Threshold + randomValue)
                        {
                            pixelColor = 255;
                        }
                        else
                        {
                            pixelColor = 0;
                        }

                        newPixel = new SKColor(pixelColor, pixelColor, pixelColor);
                    }
                    else
                    {
                        byte red = 0;
                        byte green = 0;
                        byte blue = 0;

                        if (pixel.Red > Threshold + randomValue)
                        {
                            red = 255;
                        }
                        if (pixel.Green > Threshold + randomValue)
                        {
                            green = 255;
                        }
                        if (pixel.Blue > Threshold + randomValue)
                        {
                            blue = 255;
                        }

                        newPixel = new SKColor(red, green, blue);
                    }

                    Bitmap.SetPixel(i, j, newPixel);
                }
            }
        }
    }
}
