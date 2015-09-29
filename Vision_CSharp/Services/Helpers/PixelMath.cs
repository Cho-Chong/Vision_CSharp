using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision_CSharp.Services.Helpers
{
    public static class PixelMath
    {
        public static void pixelClamp(ref double pixel)
        {
            if (pixel > 255)
            {
                pixel = 255;
            }
            else if (pixel < 0)
            {
                pixel = 0;
            }
        }

        public static double pixelMag(Color pixel)
        {
            return Math.Sqrt(pixel.R * pixel.R + pixel.B * pixel.B + pixel.G * pixel.G);
        }
    }
}
