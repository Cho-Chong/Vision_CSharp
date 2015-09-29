using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_CSharp.Model;

namespace Vision_CSharp.Services.Interfaces
{
    public interface IImageFilterService
    {
        CvImage ProcessPixels(CvImage input, IMAGE_ALGO_TYPES type);
    }
}
