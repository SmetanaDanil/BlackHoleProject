using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace BHproject
{

    class GeneticAlgorithm
    {
        public const int Neq = 3;
        public const int Bits = Neq * 32 - 1;
        public const int Bits1 = 31;
        public const int CrossingConst = 1000;
        public const double unluckiers = 0.1;
        public const double elite = 0.3;
        public const int Max = 1000;
        PrintNumb printer;
        int startN;
        int P; //P(mutation)
        double E; //convergence
        double[,] Function;
        StreamWriter writer;
        double lastfit;
       public List<double[]> individuals;
       double[] thebest;

        public GeneticAlgorithm(int startN, int P, double E, double[,] Function, PrintNumb printer)
        {
            this.P = P;
            this.startN = startN;
            this.Function = Function;
            individuals = new List<double[]>();
            this.E = E;
            this.printer = printer;

       //     writer = new StreamWriter("E:\\logs.txt");
        }

        public void GenerateNewIndividuals(int N)
        {
            double[] a = new double[Neq];

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < Neq; j++)
                    a[j] = CDll1.Rand(0.0, Max);
                
                individuals.Add(Copy(a));
            }
        }

        double[] Copy(double[] arr)
        {
            double[] res = new double[arr.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                res[i] = arr[i];
            }
            return res;
        }

        public void Crossing()
        {

            int i1, i2;
            double[] f1 = new double[Neq];
            double[] f2 = new double[Neq];

            for (int i = 0; i < CrossingConst; i++)
            {
                //choose 2 random individuals
                i1 = (int)CDll1.Rand(0, individuals.Count);
                i2 = (int)CDll1.Rand(0, individuals.Count);

                CrossingOver(individuals[i1], individuals[i2], f1, f2);

                individuals.Add(Copy(f1));
                individuals.Add(Copy(f2));
            }
        }

        public void CrossingOver(double[] p1, double[] p2, double[] f1, double[] f2)
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

            int tempp1 = (int)(p1[number_of_crossing]);
            int tempp2 = (int)(p2[number_of_crossing]);

           
            //in the bit of crossing
            int point_of_crossing = (int)CDll1.Rand(1, Bits1);
            int x = 0, y = 0;

            for (int i = point_of_crossing; i < Bits1; i++)
                x += (int)Math.Pow(2, i);

            for (int i = 0; i < point_of_crossing; i++)
                y += (int)Math.Pow(2, i);

            f1[number_of_crossing] = ((tempp1 & x) + (tempp2 & y)); 
            f2[number_of_crossing] = ((tempp2 & x) + (tempp1 & y));

        }

        public void Mutations()
        {
            int p, x = 0, y = 0;
            int number_of_mutation, point_of_mutation;
            long a;

            for (int i = 0; i < individuals.Count; i++)
            {
           
                    p = Convert.ToInt32(CDll1.Rand(0, 100));
                    if (p < P)
                    {
                        if (p % 2 == 0)
                        {
                            /*number_of_mutation = (int)CDll1.Rand(1, Neq);
                            point_of_mutation = (int)CDll1.Rand(1, Bits - 1);

                            x = (int)Math.Pow(2, point_of_mutation);
                            individuals[i][number_of_mutation] = (individuals[i][number_of_mutation] >> (point_of_mutation + 1)) << (point_of_mutation + 1) + (individuals[i][number_of_mutation] << (Bits - point_of_mutation + 2)) >> (Bits - point_of_mutation + 2) + (~(individuals[i][number_of_mutation] & x)) & x;
                             * */
                            number_of_mutation = (int)CDll1.Rand(1, Neq);
                            point_of_mutation = (int)CDll1.Rand(1, Bits1);
                            try
                            {
                                a = (int)(individuals[i][number_of_mutation]);
                                individuals.Add(Copy(individuals[i]));
                                x = 0;
                                for (int j = 0; j < point_of_mutation; j++)
                                    x += (int)Math.Pow(2, j);

                                y = 0;
                                for (int j = point_of_mutation + 1; j < Bits1; j++)
                                    y += (int)Math.Pow(2, j);

                                individuals[i][number_of_mutation] = ((x & a) + (y & a) + ((~a) & ((int)Math.Pow(2, point_of_mutation))));//has been already checked; true
                            }
                            catch
                            {
                                MessageBox.Show("Int64 переполнился :(");
                            }
                        }
                        else
                        {
                            individuals.Add(Copy(individuals[i]));
                            point_of_mutation = (int)CDll1.Rand(1, Bits1);
                            number_of_mutation = (int)CDll1.Rand(1, Neq);
                            individuals[i][number_of_mutation] += Math.Pow(2, point_of_mutation);
                        }
                    }
            }
        }

        public int Selection()
        {
            //fitness
            List<double> fitness = new List<double>();
            for (int i = 0; i < individuals.Count; i++)
                fitness.Add(dF(individuals[i]));
            //fitness.Min - the most closest
            //check convergence
            if (fitness.Min() <= E)
                return fitness.LastIndexOf(fitness.Min());

            if (lastfit < fitness.Min())
                MessageBox.Show("Что-то пошло не так...");
            lastfit = fitness.Min();
            printer(individuals[fitness.LastIndexOf(fitness.Min())], fitness.Min());
            thebest = individuals[fitness.LastIndexOf(fitness.Min())];
          /*  writer = new StreamWriter("E:\\logs.txt", true);
            writer.WriteLine("##########");
            writer.Write(individuals[fitness.LastIndexOf(fitness.Min())][0] + " ");*/
            

            int eliteIndividuals = Convert.ToInt32(individuals.Count * elite);
            int unluckyIndividuals = Convert.ToInt32(individuals.Count * unluckiers);

            List<double> TempFitness = new List<double>(fitness);
            List<double[]> Tempind = new List<double[]>();

            int ind = 0;
            
            //choose the best
            for (int i = 0; i < eliteIndividuals; i++)
            {
                ind = fitness.LastIndexOf(TempFitness.Min());
                Tempind.Add(individuals[ind]);
             //   writer.WriteLine(individuals[ind][0]);
                TempFitness.RemoveAt(TempFitness.LastIndexOf(TempFitness.Min()));
            }
          //  writer.Close();
            //choose the worst
            for (int i = 0; i < unluckyIndividuals; i++)
            {
                ind = fitness.LastIndexOf(TempFitness.Max());
                Tempind.Add(individuals[ind]);
                TempFitness.Remove(TempFitness.Max());
            }

            int new_ind = individuals.Count - Tempind.Count;

            this.individuals = new List<double[]>(Tempind);
            GenerateNewIndividuals(new_ind);
            
            return -1;
        }

        public double[] Start()
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

        public double dF(double[] arr)
        {
         //   printer(arr);
            double dy2 = 0;

            //answer: y = 2x^3 + 7x^2+65x
            for (int i = 0; i < Function.Length/2; i++)
            {
                dy2 += Math.Pow(Function[i, 1] - (arr[0] * Function[i, 0] * Function[i, 0] * Function[i, 0] + arr[1] * Function[i, 0] * Function[i, 0] + arr[2] * Function[i, 0]), 2);
            }

            return dy2;
        }

    }
}
