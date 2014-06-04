using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BHproject
{

    class KFunction
    {
        public const double aconst = 0.9981;
        public const double G = 6.67 / 100000000.0;
        public const double clight = 2.99792458 * 1000000000.0;
        public static double BHM = 3.0 * (1.99 * Math.Pow(10, 33) * G) / (clight * clight);
        public static double Rg = 2.0 * BHM;

        public static double[] Sk;
        public static double[,] RealGrapphic;

        public static void KSquareGenerate(double dr, double startr, double endr)
        {
            Sk = new double[(int)((endr - startr) / dr)];
            double f = 2.0 * Math.PI * dr;
            int j = 0;
            for (double i = startr; i <= endr; i += dr)
            {
                Sk[j] = f * (aconst * aconst + i * i) / (i * i - i * Rg + aconst * aconst);
                j++;
            }

            //1= Sk[0]

            for (int i = 0; i < Sk.Length; i++)
                Sk[i] /= Sk[0];

        }
        public static void InitializeRealGraphic()
        {
        }
    }
}
