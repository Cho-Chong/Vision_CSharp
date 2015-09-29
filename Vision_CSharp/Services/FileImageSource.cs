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
    public class FileImageSource : IImageSource
    {
        public CvImage OpenImage(string file)
        {
            CvImage result = new CvImage();

            if(file != string.Empty)
            {
                result.bitmap = new Bitmap(file);
            }

            return result;
        }

    }
}
