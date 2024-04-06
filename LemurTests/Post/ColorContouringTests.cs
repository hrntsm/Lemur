using System.Drawing;

using Xunit;

namespace Lemur.Post.ColorContour.Tests
{
    public class ColorContouringTests
    {
        [Fact]
        public void GetColor_WhenInRangeValue()
        {
            var colorContouring = new Contouring(
                (Color.White, 0),
                (Color.Black, 1)
            );

            Color getColor = colorContouring.GetColor(0.5);
            var expected = Color.FromArgb(128, 128, 128);
            Assert.Equal(expected, getColor);
        }

        [Fact]
        public void GetColor_WhenOutrangeValue()
        {
            var colorContouring = new Contouring(
                (Color.Red, 0),
                (Color.Green, 1)
            );

            Color red = colorContouring.GetColor(-1);
            Assert.Equal(Color.Red, red);
            Color green = colorContouring.GetColor(2);
            Assert.Equal(Color.Green, green);
        }
    }
}
