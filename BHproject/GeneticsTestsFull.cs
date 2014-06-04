using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BHproject
{
    class GeneticsTestsFull
    {
        public static double[] TestSquareFunc()
        {
            double[,] function = new double[22, 2];

            int j = 0;
            for (double i = -5; i <= 5; i += 0.5)
            {
                function[j, 0] = i;
                function[j, 1] = 2 * i * i + 1;
                j++;
            }

            GeneticAlgorithm gentest = new GeneticAlgorithm(30, 20, 200, function);
            return gentest.Start();
        }
    }
}
