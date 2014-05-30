using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BHproject
{

    class GeneticAlgorithm
    {


        public const int Neq = 13;
        public const int Bits = Neq * 32 - 1;
        public const int CrossingConst = 30;
        public const int thebest = 100;
        public const int unluckiers = 50;

        int startN;
        int P; //P(mutation)
        double E; //convergence

        List<int[]> individuals;

        public GeneticAlgorithm(int startN, int P, double E)
        {
            this.P = P;
            this.startN = startN;
            individuals = new List<int[]>();
        }

        public void GenerateNewIndividuals(int N)
        {
            int[] a = new int[Neq];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; i < Neq; j++)
                    a[j] = Convert.ToInt32(CDll1.Rand(0.0, 100.0));
                individuals.Add(a);
            }
        }

        public void Crossing()
        {

            int i1, i2;
            int[] f1 = new int[Neq];
            int[] f2 = new int[Neq];

            for (int i = 0; i < CrossingConst; i++)
            {
                //choose 2 random individuals
                i1 = (int)CDll1.Rand(0, startN);
                i2 = (int)CDll1.Rand(0, startN);

                CrossingOver(individuals[i1], individuals[i2], f1, f2);

                individuals.Add(f1);
                individuals.Add(f2);
            }
        }

        public void CrossingOver(int[] p1, int[] p2, int[] f1, int[] f2)
        {
            //choose a point of crossingover
            int number_of_crossing = (int)CDll1.Rand(1, Neq);
            int point_of_crossing = (int)CDll1.Rand(1, Bits - 1);

            //inside and outside the bit of crossing
            for (int i = 0; i < number_of_crossing; i++)
            {
                f1[i] = p2[i];
                f2[i] = p1[i];
            }
            for (int i = number_of_crossing + 1; i < Neq; i++)
            {
                f1[i] = p1[i];
                f2[i] = p2[i];
            }

            //in the bit of crossing
            int m = Bits - point_of_crossing;

            int x = 0, y = 0;
            for (int i = m; i < Bits; i++)
                x += (int)Math.Pow(2, i);

            for (int i = 0; i < m; i++)
                y += (int)Math.Pow(2, i);

            f1[number_of_crossing] = p1[number_of_crossing] & x + p2[number_of_crossing] & y;
            f2[number_of_crossing] = p2[number_of_crossing] & x + p2[number_of_crossing] & y;
        }

        public void Mutations()
        {
            int p, x = 0;
            int number_of_mutation, point_of_mutation;

            for (int i = 0; i < individuals.Count; i++)
            {
                p = Convert.ToInt32(CDll1.Rand(0, 100));
                if (p < P)
                {
                    if (p % 2 == 0)
                    {
                        number_of_mutation = (int)CDll1.Rand(1, Neq);
                        point_of_mutation = (int)CDll1.Rand(1, Bits - 1);

                        x = (int)Math.Pow(2, point_of_mutation);
                        individuals[i][number_of_mutation] = (individuals[i][number_of_mutation] >> (point_of_mutation + 1)) << (point_of_mutation + 1) + (individuals[i][number_of_mutation] << (Bits - point_of_mutation + 2)) >> (Bits - point_of_mutation + 2) + (~(individuals[i][number_of_mutation] & x)) & x;
                    }
                    else
                    {
                        number_of_mutation = (int)CDll1.Rand(1, Neq);
                        individuals[i][number_of_mutation] += 1;
                    }
                }
            }
        }

        public void Selection()
        {
            //fitness
            double[,] fitness = new double[individuals.Count, 2];//ОПТИМИЗИРОВАТЬ!!!
            for (int i = 0; i < individuals.Count; i++)
            {
                fitness[i, 0] = dF(individuals[i]);
                fitness[i, 1] = i;
            }
           

        }

        public int comparison(double[] x,double []y)
        {
            return (x[0] as IComparable).CompareTo(y[0]);
        }

        public double dF(int[] a)
        {
            return;
        }



    }
}
