using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_CSharp.Model;

namespace Vision_CSharp.Services.Interfaces
{
    public enum IMAGE_ALGO_TYPES
    {
        GRAYSCALE = 0,
        CANNY_EDGE,
        INVERT,
        SEPIA,
        SHARP,
        SHARP_EDGE,
        GENTLE_SHARP,
        EMBOSS,
        BLUR,
        MOTION_BLUR,
    }

    public interface IImageAlgorithm
    {
        IList<IMAGE_ALGO_TYPES> GetAlgoType();

        CvImage ProcessImage(CvImage image, IMAGE_ALGO_TYPES type);
    }
}
