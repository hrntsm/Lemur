using System.Collections.Generic;
using System.Drawing;

namespace Lemur.Post.ColorContour
{
    public static class ContoursData
    {
        public static Dictionary<string, (Color color, double position)[]> ColorContours => new Dictionary<string, (Color color, double position)[]>
        {
            ["Rainbow"] = new (Color color, double position)[]
            {
            (Color.FromArgb(255, 0, 0), 0.0),
            (Color.FromArgb(255, 63, 0), 0.11),
            (Color.FromArgb(255, 127, 0), 0.22),
            (Color.FromArgb(255, 191, 0), 0.33),
            (Color.FromArgb(255, 255, 0), 0.44),
            (Color.FromArgb(127, 255, 0), 0.55),
            (Color.FromArgb(0, 255, 0), 0.66),
            (Color.FromArgb(0, 127, 255), 0.77),
            (Color.FromArgb(0, 0, 255), 0.88),
            (Color.FromArgb(143, 0, 255), 1.0)
            },
            ["HeatMap"] = new (Color color, double position)[]
            {
            (Color.FromArgb(0, 0, 0), 0.0),
            (Color.FromArgb(0, 0, 64), 0.11),
            (Color.FromArgb(0, 0, 128), 0.22),
            (Color.FromArgb(0, 0, 192), 0.33),
            (Color.FromArgb(0, 0, 255), 0.44),
            (Color.FromArgb(0, 255, 255), 0.55),
            (Color.FromArgb(0, 255, 0), 0.66),
            (Color.FromArgb(255, 255, 0), 0.77),
            (Color.FromArgb(255, 0, 0), 0.88),
            (Color.FromArgb(255, 255, 255), 1.0)
            },
            ["Viridis"] = new (Color color, double position)[]
            {
            (Color.FromArgb(68, 1, 84), 0.0),
            (Color.FromArgb(63, 41, 111), 0.11),
            (Color.FromArgb(59, 82, 139), 0.22),
            (Color.FromArgb(46, 113, 142), 0.33),
            (Color.FromArgb(33, 145, 140), 0.44),
            (Color.FromArgb(60, 173, 120), 0.55),
            (Color.FromArgb(94, 201, 98), 0.66),
            (Color.FromArgb(158, 218, 75), 0.77),
            (Color.FromArgb(213, 236, 56), 0.88),
            (Color.FromArgb(253, 231, 37), 1.0)
            },
            ["Plasma"] = new (Color color, double position)[]
            {
            (Color.FromArgb(13, 8, 135), 0.0),
            (Color.FromArgb(69, 5, 151), 0.11),
            (Color.FromArgb(126, 3, 167), 0.22),
            (Color.FromArgb(164, 37, 143), 0.33),
            (Color.FromArgb(203, 71, 119), 0.44),
            (Color.FromArgb(225, 110, 92), 0.55),
            (Color.FromArgb(248, 149, 64), 0.66),
            (Color.FromArgb(252, 187, 48), 0.77),
            (Color.FromArgb(245, 218, 41), 0.88),
            (Color.FromArgb(239, 248, 33), 1.0)
            },
            ["Inferno"] = new (Color color, double position)[]
            {
            (Color.FromArgb(0, 0, 4), 0.0),
            (Color.FromArgb(60, 14, 15), 0.11),
            (Color.FromArgb(120, 28, 25), 0.22),
            (Color.FromArgb(177, 53, 26), 0.33),
            (Color.FromArgb(234, 79, 26), 0.44),
            (Color.FromArgb(254, 123, 43), 0.55),
            (Color.FromArgb(254, 173, 60), 0.66),
            (Color.FromArgb(254, 214, 99), 0.77),
            (Color.FromArgb(253, 234, 132), 0.88),
            (Color.FromArgb(252, 255, 164), 1.0)
            },
            ["Magma"] = new (Color color, double position)[]
            {
            (Color.FromArgb(0, 0, 4), 0.0),
            (Color.FromArgb(28, 16, 68), 0.11),
            (Color.FromArgb(79, 18, 123), 0.22),
            (Color.FromArgb(129, 37, 129), 0.33),
            (Color.FromArgb(181, 54, 122), 0.44),
            (Color.FromArgb(229, 80, 100), 0.55),
            (Color.FromArgb(251, 135, 97), 0.66),
            (Color.FromArgb(254, 194, 135), 0.77),
            (Color.FromArgb(252, 229, 179), 0.88),
            (Color.FromArgb(252, 253, 191), 1.0)
            },
            ["Grayscale"] = new (Color color, double position)[]
            {
            (Color.FromArgb(0, 0, 0), 0.0),
            (Color.FromArgb(25, 25, 25), 0.11),
            (Color.FromArgb(51, 51, 51), 0.22),
            (Color.FromArgb(76, 76, 76), 0.33),
            (Color.FromArgb(102, 102, 102), 0.44),
            (Color.FromArgb(127, 127, 127), 0.55),
            (Color.FromArgb(153, 153, 153), 0.66),
            (Color.FromArgb(178, 178, 178), 0.77),
            (Color.FromArgb(204, 204, 204), 0.88),
            (Color.FromArgb(229, 229, 229), 1.0)
            }
        };
    }
}
