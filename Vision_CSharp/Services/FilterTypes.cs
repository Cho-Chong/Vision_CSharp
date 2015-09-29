using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_CSharp.Model;
using Vision_CSharp.Services.Interfaces;

namespace Vision_CSharp.Services
{
    public class FilterTypes
    {
        private static Filter BLUR_FILTER = new Filter(
            new double[,]
            {
                                 { 0.0, 0.2,  0.0, },
                                 { 0.2, 0.2,  0.2, },
                                 { 0.0, 0.2,  0.0 }
            },
            1.0,
            0.0);

        private static Filter EMBOSS_FILTER = new Filter(
            new double[,]
            {
                                { -1, -1,  0, },
                                {-1,  0,  1,},
                                {0,  1,  1},
            },
            1.0,
            128.0);

        private static Filter MOTION_BLUR_FILTER = new Filter(
            new double[,]
            {
                        {1, 0, 0, 0, 0, 0, 0, 0, 0,},
                        {0, 1, 0, 0, 0, 0, 0, 0, 0,},
                        {0, 0, 1, 0, 0, 0, 0, 0, 0,},
                        {0, 0, 0, 1, 0, 0, 0, 0, 0,},
                        {0, 0, 0, 0, 1, 0, 0, 0, 0,},
                        {0, 0, 0, 0, 0, 1, 0, 0, 0,},
                        {0, 0, 0, 0, 0, 0, 1, 0, 0,},
                        {0, 0, 0, 0, 0, 0, 0, 1, 0,},
                        {0, 0, 0, 0, 0, 0, 0, 0, 1,}
            },
            1.0 / 9.0,
            0.0);

        private static Filter INVERT_FILTER = new Filter(
            new double[,]
            {
                        {-1,0,0,0,0 },
                        {0,-1,0,0,0 },
                        {0,0,-1,0,0 },
                        {0,0,0,1,0 },
                        {1,1,1,1,1 }
            },
            1.0,
            0.0);

        private static Filter SEPIA_FILTER = new Filter(
            new double[,]
            {
                        {0.393,0.349,0.272,0,0 },
                        {0.769,0.686,0.534,0,0 },
                        {0.189,0.168,0.131,0,0 },
                        {0,0,0,1,0 },
                        {0,0,0,0,1 }
            },
            1.0,
            0.0);

        private static Filter SHARPEN_FILTER = new Filter(
            new double[,]
            {
                         { 0.0, 0.2,  0.0, },
                         { 0.2, 0.2,  0.2, },
                         { 0.0, 0.2,  0.0 }
            },
            1.0,
            0.0);

        private static Filter SHARPEN_EDGE_FILTER = new Filter(
            new double[,]
            {
                 { 1,  1,  1, },
                 {1, -7,  1,},
                 {1,  1,  1}
            },
            1.0,
            0.0);

        private static Filter GENTLE_SHARPEN_FILTER = new Filter(
            new double[,]
            {
                                {-1, -1, -1, -1, -1, },
                                {-1,  2,  2,  2, -1,},
                                {-1,  2,  8,  2, -1,},
                                {-1,  2,  2,  2, -1,},
                                {-1, -1, -1, -1, -1,}
            },
            1.0 / 8.0,
            0.0);

        private static IDictionary<IMAGE_ALGO_TYPES, Filter> filterMap = new Dictionary<IMAGE_ALGO_TYPES, Filter>()
        {
            {IMAGE_ALGO_TYPES.BLUR, BLUR_FILTER },
            {IMAGE_ALGO_TYPES.EMBOSS, EMBOSS_FILTER },
            {IMAGE_ALGO_TYPES.MOTION_BLUR, MOTION_BLUR_FILTER },
            {IMAGE_ALGO_TYPES.INVERT, INVERT_FILTER },
            {IMAGE_ALGO_TYPES.SEPIA, SEPIA_FILTER },
            {IMAGE_ALGO_TYPES.SHARP, SHARPEN_FILTER },
            {IMAGE_ALGO_TYPES.SHARP_EDGE, SHARPEN_EDGE_FILTER },
            {IMAGE_ALGO_TYPES.GENTLE_SHARP, GENTLE_SHARPEN_FILTER },

        };

        public static Filter GetType(IMAGE_ALGO_TYPES type)
        {
            Filter value = null;

            if( filterMap.TryGetValue(type, out value))
            {
                return value;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

    }
}
