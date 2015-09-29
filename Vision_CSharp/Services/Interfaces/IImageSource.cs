using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_CSharp.Model;

namespace Vision_CSharp.Services.Interfaces
{
    public interface IImageSource
    {
        CvImage OpenImage(string file);
    }
}
