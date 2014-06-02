using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BHproject
{

    class GeneticAlgorithm
    {
        public const int Neq = 2;
        public const int Bits = Neq * 32 - 1;
        public const int Bits1 = 31;
        public const int CrossingConst = 30;
        public const double unluckiers = 0.1;
        public const double elite = 0.2;
 

        int startN;
        int P; //P(mutation)
        double E; //convergence
        int max;

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
            int number_of_crossing = (int)CDll1.Rand(0, Neq);

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

            int tempp1 = (int)(p1[number_of_crossing] * 100.0);
            int tempp2 = (int)(p2[number_of_crossing] * 100.0);

           
            //in the bit of crossing
            int point_of_crossing = (int)CDll1.Rand(1, Bits1);
            int x = 0, y = 0;

            for (int i = point_of_crossing; i < Bits1; i++)
                x += (int)Math.Pow(2, i);

            for (int i = 0; i < point_of_crossing; i++)
                y += (int)Math.Pow(2, i);

            f1[number_of_crossing] = ((tempp1 & x) + (tempp2 & y)) / 100.0;
            f2[number_of_crossing] = ((tempp2 & x) + (tempp1 & y)) / 100.0;

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

        public int Selection()//ОПТИМИЗИРОВАТЬ ЭТОТ КОШМАР
        {
            //fitness
            List<double> fitness = new List<double>();
            for (int i = 0; i < individuals.Count; i++)
                fitness.Add(dF(individuals[i]));

            //check convergence
            if (fitness.Min() <= E)
                return fitness.LastIndexOf(fitness.Min());

            int eliteIndividuals = Convert.ToInt16(individuals.Count * elite);
            int unluckyIndividuals = Convert.ToInt16(individuals.Count * unluckiers);

            List<double> TempFitness = new List<double>(fitness);
            List<int[]> Tempind = new List<int[]>();

            int ind = 0;
            
            //choose the best
            for (int i = 0; i < eliteIndividuals; i++)
            {
                ind = fitness.LastIndexOf(TempFitness.Min());
                Tempind.Add(individuals[ind]);
                TempFitness.Remove(TempFitness.Min());
            }

            //choose the worst
            for (int i = 0; i < unluckyIndividuals; i++)
            {
                ind = fitness.LastIndexOf(TempFitness.Max());
                Tempind.Add(individuals[ind]);
                TempFitness.Remove(TempFitness.Max());
            }

            int new_ind = individuals.Count - Tempind.Count;

            this.individuals = new List<int[]>(Tempind);
            GenerateNewIndividuals(new_ind);

            return -1;
        }

        public int[] Start()
        {
            GenerateNewIndividuals(startN);
            Crossing();
            Mutations();

            int result = Selection();

            while (result == -1)
            {
                Crossing();
                Mutations();
                result = Selection();
            }

            return individuals[result];
        }

        public double dF(int[] a)
        {
            //y = x^2 + 1

            return 0.0;
        }

    }
}
