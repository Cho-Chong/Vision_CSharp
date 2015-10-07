using ChongCore.Model;
using ChongCore.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vision_CSharp.Services;
using Vision_CSharp.Services.Interfaces;

namespace Vision_CSharp.ViewModel
{
    public class ImageVM : BaseVM
    {
        string IMG_RES_DIR = "C:\\Users\\Spam\\Documents\\Visual Studio 2015\\Projects\\Resources\\Images\\";
        string BACKUP_APPEND = "backup";
        string LENNA_IMG_NAME = "lenna.png";
        string VALVE_IMG_NAME = "valve.png";
        string FLOWER_IMG_NAME = "flower.png";
        string SAVE_OPTIONS = "PNG (.png)|*.png";
        string DEFAULT_EXT = ".png";

        #region UI_VARS

        private string imagePath;
        public string ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                SetProperty(ref imagePath, value, forceSet: true);
            }
        }

        public ICommand OpenImage
        {
            get;
            set;
        }

        public ICommand SaveImage
        {
            get;
            set;
        }

        public ICommand ProcessImage
        {
            get;
            set;
        }

        private IImageFilterService imageService;

        #endregion UI_VARS

        //TODO: inject algos
        public ImageVM()
        {
            this.imagePath = IMG_RES_DIR + LENNA_IMG_NAME;
            this.OpenImage = new RelayCommand((l) => true, OpenFile);
            this.SaveImage = new RelayCommand((l) => true, SaveFile);
            this.ProcessImage = new RelayCommand((l) => true, processImageAlgo);
            this.imageService = new ImageFilterService();
            CopyImage();
        }

        private void CopyImage()
        {
            if (File.Exists(this.imagePath))
            {
                var bitmap = new Bitmap(this.ImagePath);
                string[] path_and_name = this.ImagePath.Split('.');

                string working_path = path_and_name.First() + BACKUP_APPEND + '.' + path_and_name.Last();
                bitmap.Save(working_path);
                this.ImagePath = working_path;
            }
        }

        private void processImageAlgo(object algo_type)
        {
            Task.Factory.StartNew(() =>
            {
                //TODO: can race condition with itself...
                if (algo_type is IMAGE_ALGO_TYPES)
                {
                    var path = this.imagePath;
                    CloseImage();
                    var source = new FileImageSource();
                    var image = source.OpenImage(path);
                    var processed_image = imageService.ProcessPixels(image, (IMAGE_ALGO_TYPES)algo_type);
                    image.bitmap.Dispose();
                    processed_image.bitmap.Save(path);
                    OpenImagePath(path);
                    //if (service != null)
                    //{
                    //    var path = this.imagePath;
                    //    CloseImage();
                    //    var source = new FileImageSource();
                    //    var image = source.OpenImage(path);
                    //    var processed_image = service.ProcessImage(image);
                    //    var temp = new Bitmap(processed_image.bitmap);
                    //    //CHO_ACTIVE: OH GOD NO
                    //    image.bitmap.Dispose();
                    //    processed_image.bitmap.Save(path);
                    //    OpenImagePath(path);
                    //}
                } 

            });
        }

        private void SaveFile(object obj)
        {
            var dialog = new SaveFileDialog();
            dialog.FileName = imagePath;
            dialog.DefaultExt = DEFAULT_EXT;
            dialog.Filter = SAVE_OPTIONS;

            if(dialog.ShowDialog() == true)
            {

            }
        }

        private void OpenFile(object obj)
        {
            var dialog = new OpenFileDialog();
            if( dialog.ShowDialog() == true)
            {
                imagePath = dialog.FileName;
                CopyImage();
            }
        }

        private void OpenImagePath(string path)
        {
            ImagePath = path;
        }

        private void CloseImage()
        {
            ImagePath = string.Empty;
        }
    }
}
