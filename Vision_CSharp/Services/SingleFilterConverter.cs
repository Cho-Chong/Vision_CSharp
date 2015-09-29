using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_CSharp.Model;
using Vision_CSharp.Services.Helpers;
using Vision_CSharp.Services.Interfaces;

namespace Vision_CSharp.Services
{
    public class SingleFilterConverter : IImageAlgorithm
    {
        public IList<IMAGE_ALGO_TYPES> GetAlgoType()
        {
            return new List<IMAGE_ALGO_TYPES>()
            {
               IMAGE_ALGO_TYPES.SEPIA,
               IMAGE_ALGO_TYPES.BLUR,
               IMAGE_ALGO_TYPES.MOTION_BLUR,
               IMAGE_ALGO_TYPES.INVERT,
               IMAGE_ALGO_TYPES.SHARP,
               IMAGE_ALGO_TYPES.SHARP_EDGE,
               IMAGE_ALGO_TYPES.GENTLE_SHARP,
               IMAGE_ALGO_TYPES.EMBOSS,
            };
        }

        public CvImage ProcessImage(CvImage image, IMAGE_ALGO_TYPES type)
        {
            return MatrixMath.Convolve(FilterTypes.GetType(type), image.bitmap);
        }
    }
}
