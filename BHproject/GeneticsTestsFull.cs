using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BHproject
{
    public delegate void PrintNumb(double[] arr, double dy);
    class GeneticsTestsFull
    {
        public static double[] TestSquareFunc(PrintNumb printer)
        {
            double[,] function = new double[21, 2];

            int j = 0;
            for (double i = -5; i <= 5; i += 0.5)
            {
                function[j, 0] = i;
                function[j, 1] = 2.0 * i * i* i + 65.0*i + 7.0*i*i;
                j++;
            }

            GeneticAlgorithm gentest = new GeneticAlgorithm(500, 30, 50, function, printer);
            return gentest.Start();
        }
    }
}
