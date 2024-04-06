using System;
using System.Drawing;
using System.Linq;

namespace Lemur.Post.ColorContour
{
    public class Contouring
    {
        private readonly (Color color, double position)[] _colors;

        public Contouring(params (Color color, double position)[] colors)
        {
            _colors = colors;
        }

        public Color GetColor(double value)
        {
            if (value <= 0)
            {
                return _colors[0].color;
            }
            if (value >= 1)
            {
                return _colors.Last().color;
            }

            for (int i = 0; i < _colors.Length - 1; i++)
            {
                if (value >= _colors[i].position && value <= _colors[i + 1].position)
                {
                    double t = (value - _colors[i].position) / (_colors[i + 1].position - _colors[i].position);
                    return Color.FromArgb(
                        (int)Math.Round(_colors[i].color.R + (_colors[i + 1].color.R - _colors[i].color.R) * t),
                        (int)Math.Round(_colors[i].color.G + (_colors[i + 1].color.G - _colors[i].color.G) * t),
                        (int)Math.Round(_colors[i].color.B + (_colors[i + 1].color.B - _colors[i].color.B) * t));
                }
            }

            throw new ArgumentOutOfRangeException(nameof(value), "Invalid value for color contouring.");
        }

        public static Contouring CreateJet()
        {
            return new Contouring(
            );
        }
    }
}
