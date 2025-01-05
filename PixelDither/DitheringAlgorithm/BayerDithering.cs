using SkiaSharp;

namespace PixelDither.DitheringAlgorithm
{
    public class BayerDithering : Dithering
    {
        public override string FilePath { get; set; }
        public override SKBitmap Bitmap { get; set; }
        public override string NewFilePath { get; set; }

        public BayerDithering(string filePath, string newFilePath)
        {
            Bitmap = SKBitmap.Decode(filePath);

            FilePath = filePath;
            NewFilePath = newFilePath;
        }

        public BayerDithering(SKBitmap bitmap, string newFilePath)
        {
            Bitmap = bitmap;

            NewFilePath = newFilePath;
        }

        public override void Dither()
        {

        }
    }
}
