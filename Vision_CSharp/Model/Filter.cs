using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision_CSharp.Model;
using Vision_CSharp.Services.Helpers;

namespace Vision_CSharp.Model
{
    public class Filter
    {
        public readonly double[,] MATRIX;
        public readonly double factor;
        public readonly double bias;

        public Filter(double[,] matrix, double factor, double bias)
        {
            int wid = matrix.GetLength(0);
            int hei = matrix.GetLength(0);

            MATRIX = new double[wid, hei];

            Array.Copy(matrix, 0, MATRIX, 0, matrix.Length);

            this.factor = factor;
            this.bias = bias;
        }
    }
}
