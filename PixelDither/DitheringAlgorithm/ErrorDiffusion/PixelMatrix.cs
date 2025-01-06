using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelDither.DitheringAlgorithm
{
    public class PixelMask(int offsetX, int offsetY, double factor)
    {
        public int OffsetX { get; set; } = offsetX;
        public int OffsetY { get; set; } = offsetY;
        public double Factor { get; set; } = factor;
    }
}
