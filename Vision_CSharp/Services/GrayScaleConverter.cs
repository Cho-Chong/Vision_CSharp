using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_CSharp.Model;
using Vision_CSharp.Services.Interfaces;

namespace Vision_CSharp.Services
{
    public class GrayScaleConverter : IImageAlgorithm
    {
        private int GRAY_SCALE_THRESHOLD = (255 / 2);
        private int MAX_MIX = (255 * 3);
        private double RED_SCALE = 0.2989;
        private double GREEN_SCALE = 0.5870;
        private double BLUE_SCALE = 0.1140;


        public IList<IMAGE_ALGO_TYPES> GetAlgoType()
        {
            return new List<IMAGE_ALGO_TYPES>() { IMAGE_ALGO_TYPES.GRAYSCALE };
        }

        public CvImage ProcessImage(CvImage image, IMAGE_ALGO_TYPES type)
        {
            CvImage result = new CvImage();

            result.bitmap = new Bitmap(image.bitmap.Width, image.bitmap.Height);

            for (int w_ic = 0; w_ic < image.bitmap.Width; w_ic++)
            {
                for (int h_ic = 0; h_ic < image.bitmap.Height; h_ic++)
                {
                    Color pix_color = image.bitmap.GetPixel(w_ic, h_ic);

                    var gray_scale = (int) (pix_color.R* RED_SCALE + pix_color.B * BLUE_SCALE + pix_color.G * GREEN_SCALE);

                    result.bitmap.SetPixel(w_ic, h_ic, Color.FromArgb(pix_color.A, gray_scale, gray_scale, gray_scale));
                }

            }

            return result;
        }
    }
}
