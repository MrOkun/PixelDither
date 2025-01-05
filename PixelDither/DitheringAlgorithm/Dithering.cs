﻿using SkiaSharp;

namespace PixelDither.DitheringAlgorithm
{
    public abstract class Dithering
    {
        public virtual string FilePath { get; set; }
        public virtual SKBitmap Bitmap { get; set; }
        public virtual string NewFilePath { get; set; }

        public virtual void Dither() { }
        public void Save()
        {
            var encoded = Bitmap.Encode(SKEncodedImageFormat.Png, 8);
            using var stream = new FileStream(NewFilePath, FileMode.Create, FileAccess.Write);
            encoded.SaveTo(stream);
        }
    }
}
