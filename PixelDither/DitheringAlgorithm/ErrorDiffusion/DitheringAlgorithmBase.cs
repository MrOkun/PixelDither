using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelDither.DitheringAlgorithm.ErrorDiffusion
{
    public class DitheringAlgorithmBase
    {
        public PixelMask[] Simple2D()
        {
            return [
                new PixelMask(1, 0, 1.0)
            ];
        }

        public PixelMask[] FloydSteinberg()
        {
            return [
                new PixelMask(1, 0, 7/16.0),
                new PixelMask(-1, 1, 3/16.0),
                new PixelMask(0, 1, 5/16.0),
                new PixelMask(1, 1, 1/16.0)
            ];
        }

        public PixelMask[] FalseFloydSteinberg()
        {
            return [
                new PixelMask(1, 0, 3 / 8.0),
                new PixelMask(0, 1, 3 / 8.0),
                new PixelMask(1, 1, 2 / 8.0)
            ];
        }

        public PixelMask[] JarvisJudiceNinke()
        {
            return
            [
                new PixelMask(1, 0, 7 / 48.0),
                new PixelMask(2, 0, 5 / 48.0),
                new PixelMask(-2, 1, 3 / 48.0),
                new PixelMask(-1, 1, 5 / 48.0),
                new PixelMask(0, 1, 7 / 48.0),
                new PixelMask(1, 1, 5 / 48.0),
                new PixelMask(2, 1, 3 / 48.0),
                new PixelMask(-2, 2, 1 / 48.0),
                new PixelMask(-1, 2, 3 / 48.0),
                new PixelMask(0, 2, 5 / 48.0),
                new PixelMask(1, 2, 3 / 48.0),
                new PixelMask(2, 2, 1 / 48.0)
            ];
        }

        public PixelMask[] Atkinson()
        {
            return
            [
                new PixelMask(1, 0, 1 / 8.0),
                new PixelMask(2, 0, 1 / 8.0),
                new PixelMask(-1, 1, 1 / 8.0),
                new PixelMask(0, 1, 1 / 8.0),
                new PixelMask(1, 1, 1 / 8.0),
                new PixelMask(0, 2, 1 / 8.0)
            ];
        }

        public PixelMask[] Stucki()
        {
            return
            [
                new PixelMask(1, 0, 8 / 42.0),
                new PixelMask(2, 0, 4 / 42.0),
                new PixelMask(-2, 1, 2 / 42.0),
                new PixelMask(-1, 1, 4 / 42.0),
                new PixelMask(0, 1, 8 / 42.0),
                new PixelMask(1, 1, 4 / 42.0),
                new PixelMask(2, 1, 2 / 42.0),
                new PixelMask(-2, 2, 1 / 42.0),
                new PixelMask(-1, 2, 2 / 42.0),
                new PixelMask(0, 2, 4 / 42.0),
                new PixelMask(1, 2, 2 / 42.0),
                new PixelMask(2, 2, 1 / 42.0)
            ];
        }

        public PixelMask[] Burkes()
        {
            return
            [
                new PixelMask(1, 0, 8 / 32.0),
                new PixelMask(2, 0, 4 / 32.0),
                new PixelMask(-2, 1, 2 / 32.0),
                new PixelMask(-1, 1, 4 / 32.0),
                new PixelMask(0, 1, 8 / 32.0),
                new PixelMask(1, 1, 4 / 32.0),
                new PixelMask(2, 1, 2 / 32.0)
            ];
        }

        public PixelMask[] Sierra3()
        {
            return
            [
                new PixelMask(1, 0, 5 / 32.0),
                new PixelMask(2, 0, 3 / 32.0),
                new PixelMask(-2, 1, 2 / 32.0),
                new PixelMask(-1, 1, 4 / 32.0),
                new PixelMask(0, 1, 5 / 32.0),
                new PixelMask(1, 1, 4 / 32.0),
                new PixelMask(2, 1, 2 / 32.0),
                new PixelMask(-1, 2, 2 / 32.0),
                new PixelMask(0, 2, 3 / 32.0),
                new PixelMask(1, 2, 2 / 32.0)
            ];
        }

        public PixelMask[] Sierra2()
        {
            return
            [
                new PixelMask(1, 0, 4 / 16.0),
                new PixelMask(2, 0, 3 / 16.0),
                new PixelMask(-1, 1, 1 / 16.0),
                new PixelMask(0, 1, 2 / 16.0),
                new PixelMask(1, 1, 3 / 16.0),
                new PixelMask(2, 1, 2 / 16.0),
                new PixelMask(-1, 2, 1 / 16.0),
                new PixelMask(0, 2, 2 / 16.0),
                new PixelMask(1, 2, 1 / 16.0)
            ];
        }

        public PixelMask[] SierraLite()
        {
            return
            [
                new PixelMask(1, 0, 2 / 4.0),
                new PixelMask(0, 1, 1 / 4.0),
                new PixelMask(1, 1, 1 / 4.0)
            ];
        }

        public PixelMask[] StevenPigeon()
        {
            return
            [
                new PixelMask(1, 0, 7 / 16.0),
                new PixelMask(2, 0, 5 / 16.0),
                new PixelMask(-2, 1, 3 / 16.0),
                new PixelMask(-1, 1, 5 / 16.0),
                new PixelMask(0, 1, 7 / 16.0),
                new PixelMask(1, 1, 5 / 16.0),
                new PixelMask(2, 1, 3 / 16.0),
                new PixelMask(-1, 2, 3 / 16.0),
                new PixelMask(0, 2, 5 / 16.0),
                new PixelMask(1, 2, 3 / 16.0)
            ];
        }
    }
}
