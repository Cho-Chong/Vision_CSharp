using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_CSharp.Model;
using Vision_CSharp.Services.Interfaces;

namespace Vision_CSharp.Services
{
    public class ImageFilterService : IImageFilterService
    {
        IList<IImageAlgorithm> algoList;

        public ImageFilterService()
        {
            algoList = new List<IImageAlgorithm>()
            {
                new CannyEdgeDetector(),
                new GrayScaleConverter(),
                new SingleFilterConverter(),
            };
        }

        public CvImage ProcessPixels(CvImage input, IMAGE_ALGO_TYPES type)
        {
            CvImage result = null;

            try
            {
                var algorithm = algoList.First(a => a != null && a.GetAlgoType().Any(t => t == type));

                result = algorithm.ProcessImage(input, type);

            }
            catch(Exception ex)
            {
                
            }

            return result;
        }
    }
}
