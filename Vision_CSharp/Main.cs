using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_CSharp.Services;

namespace Vision_CSharp
{
    public class App : System.Windows.Application
    {

        [STAThread]
        public static void Main(params string[] args)
        {
            //string IMG_RES_DIR = "C:\\Users\\Spam\\Documents\\Visual Studio 2015\\Projects\\Resources\\Images\\";
            //string LENNA_IMG_NAME = "lenna.png";
            //string VALVE_IMG_NAME = "valve.png";
            //string IMG_NAME = VALVE_IMG_NAME;

            var app = new App();
            app.StartupUri = new System.Uri("View\\MainWindow.xaml", System.UriKind.Relative);

            //FileImageSource source = new FileImageSource();
            //var image = source.OpenImage(IMG_RES_DIR + IMG_NAME);
            //GrayScaleConverter gsConverter = new GrayScaleConverter();

            //var gray_image = gsConverter.ProcessImage(image);
            //gray_image.bitmap.Save(IMG_RES_DIR + "Gray" + IMG_NAME);

            //CannyEdgeDetector ced = new CannyEdgeDetector();
            //var ced_image = ced.ProcessImage(image);
            //ced_image.bitmap.Save(IMG_RES_DIR + "CED" + IMG_NAME);

            app.Run();
        }
    }
}
