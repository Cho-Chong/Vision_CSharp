using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ChongCore.Model.Interfaces;

namespace Vision_CSharp.Model
{
    public class CvImage : Bindable
    {
        public Bitmap bitmap;

        //TODO: REFACTOR TO USE GENERIC PIXELS INSTEAD
        public Pixel[,] pixels;

        public CvImage()
        {

        }
    }
}
