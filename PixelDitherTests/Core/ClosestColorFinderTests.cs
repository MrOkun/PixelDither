using PixelDither.Core;
using SkiaSharp;

namespace PixelDitherTests.Core
{
    [TestClass()]
    public class ClosestColorFinderTests
    {
        [TestMethod()]
        public void FindClosestColor_Euclidean_ReturnsCorrectColor()
        {
            var closestColorFinder = new ClosestColorFinder();
            var originalColor = new SKColor(100, 150, 200);
            var palette = new List<SKColor>
            {
                new SKColor(101, 151, 201),
                new SKColor(120, 140, 180),
                new SKColor(80, 160, 220)
            };

            var closestColor = closestColorFinder.FindClosestColor(originalColor, palette, DistanceMethod.Euclidean);

            Assert.AreEqual(new SKColor(101, 151, 201), closestColor);
        }

        [TestMethod()]
        public void FindClosestColor_Manhattan_ReturnsCorrectColor()
        {
            var closestColorFinder = new ClosestColorFinder();
            var originalColor = new SKColor(100, 150, 200);
            var palette = new List<SKColor>
            {
                new SKColor(101, 151, 201),
                new SKColor(120, 140, 180),
                new SKColor(80, 160, 220)
            };

            var closestColor = closestColorFinder.FindClosestColor(originalColor, palette, DistanceMethod.Manhattan);

            Assert.AreEqual(new SKColor(101, 151, 201), closestColor);
        }

        [TestMethod()]
        public void FindClosestColor_DeltaRGB_ReturnsCorrectColor()
        {
            var closestColorFinder = new ClosestColorFinder();
            var originalColor = new SKColor(100, 150, 200);
            var palette = new List<SKColor>
            {
                new SKColor(101, 151, 201),
                new SKColor(120, 140, 180),
                new SKColor(80, 160, 220)
            };

            var closestColor = closestColorFinder.FindClosestColor(originalColor, palette, DistanceMethod.DeltaRGB);

            Assert.AreEqual(new SKColor(101, 151, 201), closestColor);
        }

        [TestMethod()]
        public void CalculateEuclideanDistance_ReturnsCorrectColor()
        {
            var closestColorFinder = new ClosestColorFinder();
            var color1 = new SKColor(100, 150, 200);
            var color2 = new SKColor(150, 0, 0);

            var closestColor = closestColorFinder.CalculateEuclideanDistance(color1, color2);

            Assert.AreEqual(254.95097567963924, closestColor);
        }

        [TestMethod()]
        public void FindClosestColor_Quantization_ReturnsCorrectColor()
        {
            var closestColorFinder = new ClosestColorFinder();
            var originalColor = new SKColor(123, 234, 12);
            int factor = 10;

            var quantizedColor = closestColorFinder.FindClosestColor(originalColor, factor);

            Assert.AreEqual(new SKColor(120, 230, 10), quantizedColor);
        }

        [TestMethod()]
        public void CalculateManhattanDistance_ReturnsCorrectDistance()
        {
            var closestColorFinder = new ClosestColorFinder();
            var color1 = new SKColor(100, 150, 200);
            var color2 = new SKColor(150, 100, 50);

            var distance = closestColorFinder.CalculateManhattanDistance(color1, color2);

            Assert.AreEqual(250, distance);
        }

        [TestMethod()]
        public void CalculateDeltaRGBDistance_ReturnsCorrectDistance()
        {
            var closestColorFinder = new ClosestColorFinder();
            var color1 = new SKColor(100, 150, 200);
            var color2 = new SKColor(150, 100, 50);

            var distance = closestColorFinder.CalculateDeltaRGBDistance(color1, color2);

            Assert.AreEqual(150, distance);
        }

        [TestMethod()]
        public void QuantizeChannel_ReturnsCorrectQuantizedValue()
        {
            var closestColorFinder = new ClosestColorFinder();
            byte channel = 123;
            int factor = 10;

            var quantizedValue = closestColorFinder.QuantizeChannel(channel, factor);

            Assert.AreEqual(120, quantizedValue);
        }

        [TestMethod()]
        public void QuantizeChannel_ClampsToMaxValue()
        {
            var closestColorFinder = new ClosestColorFinder();
            byte channel = 250;
            int factor = 300;

            var quantizedValue = closestColorFinder.QuantizeChannel(channel, factor);

            Assert.AreEqual(255, quantizedValue);
        }
    }
}