using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_CSharp.Model;
using Vision_CSharp.Services.Helpers;
using Vision_CSharp.Services.Interfaces;

namespace Vision_CSharp.Services
{
    public class CannyEdgeDetector : IImageAlgorithm
    {
        private Filter GAUSSIAN_FILTER = new Filter(
            new double[,]
            {
                { 2, 4, 5, 4, 2 },
                { 4, 9, 12, 9, 4 },
                { 5, 12, 15, 12, 5 },
                { 4, 9, 12, 9, 4 },
                { 2, 4, 5, 4, 2 }
            },
            (1.0/115.0),
            0.0
            );

        private Filter SOBEL_X_FILTER = new Filter(
            new double[,]
            {
                {-1, 0, 1 },
                {-2, 0, 2 },
                {-1, 0, 1 }
            },
            1.0,
            0.0
            );

        private Filter SOBEL_Y_FILTER = new Filter(
            new double[,]
            {
                {1, 2, 1 },
                {0, 0, 0 },
                {-1, -2, -1 }
            },
            1.0,
            0.0
            );

        private int EDGE_THRESHOLD_UPPER = 180;
        private int EDGE_THRESHOLD_LOWER = 120;

        public IList<IMAGE_ALGO_TYPES> GetAlgoType()
        {
            return new List<IMAGE_ALGO_TYPES>() { IMAGE_ALGO_TYPES.CANNY_EDGE };
        }

        public CvImage ProcessImage(CvImage image, IMAGE_ALGO_TYPES type)
        {
            var gausianed = MatrixMath.Convolve(GAUSSIAN_FILTER, image.bitmap);
            var sobel_xed = MatrixMath.Convolve(SOBEL_X_FILTER, gausianed.bitmap);
            var sobel_yed = MatrixMath.Convolve(SOBEL_Y_FILTER, gausianed.bitmap);
            var canny_edged_image = DrawCannyEdges(sobel_xed.bitmap, sobel_yed.bitmap);

            return canny_edged_image;
        }

        private CvImage DrawCannyEdges(Bitmap sobel_x, Bitmap sobel_y)
        {
            CvImage image = new CvImage();

            int[,] edge_direction = new int[sobel_x.Width, sobel_x.Height];
            int[,] edge_mag = new int[sobel_x.Width, sobel_x.Height];
            int[,] non_max = new int[sobel_x.Width, sobel_x.Height];
            int kernWid = GAUSSIAN_FILTER.MATRIX.GetLength(1);
            int kernHei = GAUSSIAN_FILTER.MATRIX.GetLength(0);
            int kernOffset = (kernWid - 1) / 2;

            for (int wid_ic = kernOffset; wid_ic < sobel_x.Width - kernOffset; wid_ic++)
            {
                for (int hei_ic = kernOffset; hei_ic < sobel_x.Height - kernOffset; hei_ic++)
                {
                    var pixel_y = sobel_y.GetPixel(wid_ic, hei_ic);
                    var pixel_x = sobel_x.GetPixel(wid_ic, hei_ic);
                    var mag_y = PixelMath.pixelMag(pixel_y);
                    var mag_x = PixelMath.pixelMag(pixel_y);
                    var edge_strength = mag_y + mag_x;

                    edge_mag[wid_ic, hei_ic] = (int)edge_strength;
                    non_max[wid_ic, hei_ic] = (int)edge_strength;
                }
            }

            for (int wid_ic = kernWid; wid_ic < sobel_x.Width - kernWid; wid_ic++)
            {
                for(int hei_ic = kernHei; hei_ic < sobel_x.Height - kernHei; hei_ic++)
                {
                    var pixel_y = sobel_y.GetPixel(wid_ic, hei_ic);
                    var pixel_x = sobel_x.GetPixel(wid_ic, hei_ic);
                    var mag_y = PixelMath.pixelMag(pixel_y);
                    var mag_x = PixelMath.pixelMag(pixel_y);
                    var current_mag = edge_mag[wid_ic, hei_ic];
                    double theta = current_mag != 0 ? Math.Atan2(mag_y, mag_x) : 90;

                    //CHO_ACTIVE: clean this up
                    //near horizontal
                    if ((theta > -22.5 && theta <= 22.5) ||
                        (theta > 157.5) ||
                        (theta < -157.5))
                    {
                        if((current_mag < edge_mag[wid_ic,hei_ic + 1]) ||
                            (current_mag < edge_mag[wid_ic, hei_ic - 1]))
                        {
                            non_max[wid_ic, hei_ic] = 0;
                        }

                    }
                    // +45 degree
                    else if ((theta > 22.5 && theta <= 67.5) ||
                             (theta <= -67.5 && theta > -22.5))
                    {
                        if ((current_mag < edge_mag[wid_ic + 1, hei_ic + 1]) ||
                            (current_mag < edge_mag[wid_ic - 1, hei_ic - 1]))
                        {
                            non_max[wid_ic, hei_ic] = 0;
                        }
                    }
                    //vertical
                    else if ((theta > 67.5 && theta <= 112.5) ||
                             (theta <= -67.5 && theta > -112.5))
                    {
                        if ((current_mag < edge_mag[wid_ic + 1, hei_ic]) ||
                            (current_mag < edge_mag[wid_ic - 1, hei_ic]))
                        {
                            non_max[wid_ic, hei_ic] = 0;
                        }
                    }
                    // +135 degree
                    else
                    {
                        if ((current_mag < edge_mag[wid_ic + 1, hei_ic - 1]) ||
                            (current_mag < edge_mag[wid_ic - 1, hei_ic + 1]))
                        {
                            non_max[wid_ic, hei_ic] = 0;
                        }
                    }

                    edge_direction[wid_ic, hei_ic] = (int)theta;
                }
            }

            image.bitmap = TraceCannyEdge(non_max);

            return image;
        }

        private Bitmap TraceCannyEdge(int[,] edges)
        {
            int imgWid = edges.GetLength(0);
            int imgHei = edges.GetLength(1);
            Bitmap canny_edged = new Bitmap(imgWid, imgHei);
            bool[,] visited = new bool[imgWid, imgHei];
            bool[,] result = new bool[imgWid, imgHei];
            int kernWid = GAUSSIAN_FILTER.MATRIX.GetLength(0);
            int kernHei = GAUSSIAN_FILTER.MATRIX.GetLength(1);
            int kernOffset = (kernWid - 1) /2;

            for (int wid_ic = kernOffset; wid_ic < imgWid - kernOffset; wid_ic++)
            {
                for(int hei_ic = kernOffset; hei_ic < imgHei - kernOffset; hei_ic++)
                {
                    if (!visited[wid_ic,hei_ic])
                    {
                        bool is_good = edges[wid_ic, hei_ic] > EDGE_THRESHOLD_UPPER;
                        result[wid_ic, hei_ic] = is_good;
                        visited[wid_ic, hei_ic] = true;
                        if (is_good)
                        {
                            MarkEdges(visited, edges, result, wid_ic, hei_ic, is_good);
                        }
                    }
                }
            }


            BitmapData dstdata = canny_edged.LockBits(new Rectangle(0,0, imgWid, imgHei), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            int srcStride = dstdata.Stride;
            var bytesPerPixel = srcStride / imgWid;

            unsafe
            {
                byte* dstPointer = (byte*)dstdata.Scan0;

                for (int hei_ic = kernHei; hei_ic < imgHei - kernWid; hei_ic++)
                {
                    var row = dstPointer + (hei_ic) * srcStride;

                    for (int wid_ic = kernHei; wid_ic < imgWid - kernHei; wid_ic++)
                    {
                        var pixel = row + (wid_ic) * bytesPerPixel;
                        var fill = (byte)(result[wid_ic, hei_ic] ? 0xFF : 0x00);
                        pixel[0] = fill;
                        pixel[1] = fill;
                        pixel[2] = fill;
                        pixel[3] = 0xFF;
                    }
                }
            }

            canny_edged.UnlockBits(dstdata);

            return canny_edged;
        }

        private void MarkEdges(bool[,] visited, int[,] edges, bool[,] result, int x, int y, bool was_good)
        {
            for(int wid_ic = -1; wid_ic <= 1; wid_ic++)
            {
                for (int hei_ic = -1; hei_ic <= 1; hei_ic++)
                {
                    if ((x + wid_ic > 0 && y + hei_ic > 0) &&
                        (x + wid_ic < edges.GetLength(0)) && (y + hei_ic < edges.GetLength(1)))
                    {
                        if (!visited[x + wid_ic, y + hei_ic])
                        {
                            bool is_good = was_good ? edges[x + wid_ic, y + hei_ic] > EDGE_THRESHOLD_LOWER : edges[x + wid_ic, y + hei_ic] > EDGE_THRESHOLD_UPPER;
                            visited[x + wid_ic, y + hei_ic] = true;
                            result[x + wid_ic, y + hei_ic] = is_good;
                            if(is_good)
                            {
                                MarkEdges(visited, edges, result, x + wid_ic, y + hei_ic, is_good);
                            }
                        }
                    }
                }
            }
        }
    }
}
