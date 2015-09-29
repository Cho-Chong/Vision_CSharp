using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_CSharp.Model;
using Vision_CSharp.Services.Interfaces;

namespace Vision_CSharp.Services.Helpers
{
    public class MatrixMath
    {
        public static CvImage Convolve(Filter filter, Bitmap image)
        {
            CvImage result = new CvImage();
            var kernel = filter.MATRIX;
            var scale = filter.factor;
            var bias = filter.bias;

            result.bitmap = new Bitmap(image.Width, image.Height);
            int kernWid = kernel.GetLength(1);
            int kernHei = kernel.GetLength(0);
            var tile = new Rectangle(0, 0, image.Width, image.Height);
            BitmapData srcdata = image.LockBits(tile, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            BitmapData dstdata = result.bitmap.LockBits(tile, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int srcStride = srcdata.Stride;
            int dstStride = dstdata.Stride;
            var bytesPerPixel = srcStride / image.Width;

            // we can't filter pixels without enough surrounding pixels
            int kernOffset = (kernWid - 1) / 2;

            //TODO: does not set non-filtered pixels
            unsafe
            {
                byte* dstPointer = (byte*)dstdata.Scan0;
                byte* srcPointer = (byte*)srcdata.Scan0;
                double[] sums = new double[bytesPerPixel];

                for (int offsetY = kernOffset; offsetY < image.Height - kernOffset; offsetY++)
                {
                    for (int offsetX = kernOffset; offsetX < image.Width - kernOffset; offsetX++)
                    {
                        for (int sum_ic = 0; sum_ic < bytesPerPixel; sum_ic++)
                        {
                            sums[sum_ic] = 0;
                        }

                        var dst_row = dstPointer + offsetY * srcStride;
                        var dst_pixel = dst_row + offsetX * bytesPerPixel;

                        for (int kernY = -kernOffset; kernY <= kernOffset; kernY++)
                        {
                            var row = srcPointer + (kernY + offsetY) * srcStride;
                            
                            for (int kernX = -kernOffset; kernX <= kernOffset; kernX++)
                            {
                                var pixel = row + (kernX + offsetX) * bytesPerPixel;
                                double filter_coeff = (kernel[kernX + kernOffset, kernY + kernOffset]);
                                sums[0] += (((double)pixel[0]) * filter_coeff);
                                sums[1] += (((double)pixel[1]) * filter_coeff);
                                sums[2] += (((double)pixel[2]) * filter_coeff);
                                sums[3] = pixel[3];
                            }
                        }

                        sums[0] *= scale;
                        sums[1] *= scale;
                        sums[2] *= scale;
                        sums[0] += bias;
                        sums[1] += bias;
                        sums[2] += bias;

                        PixelMath.pixelClamp(ref sums[0]);
                        PixelMath.pixelClamp(ref sums[1]);
                        PixelMath.pixelClamp(ref sums[2]);

                        dst_pixel[0] = (byte)sums[0];
                        dst_pixel[1] = (byte)sums[1];
                        dst_pixel[2] = (byte)sums[2];
                        dst_pixel[3] = (byte)sums[3];
                    }
                }
            }

            result.bitmap.UnlockBits(dstdata);
            image.UnlockBits(srcdata);

            return result;
        }

    }
}
