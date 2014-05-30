using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BHproject
{
    public partial class Form1 : Form
    {
        DateTime dt;
        GraphPane gr;
        List<ObjectsDB> GraphicsResults;
        bool fail;
        int eq;

        public Form1()
        {
            InitializeComponent();
            fail = true;
            GraphicsResults = new List<ObjectsDB>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PointPairList list = new PointPairList();
            double radvec = Convert.ToDouble(textBox2.Text);
            double point_of_view = Convert.ToDouble(textBox1.Text);
            int photon_amount = int.Parse(textBox3.Text) * 1000;

            RandomSphere random = new RandomSphere(null);

            double[,] arr = random.RandomizeForSphere(photon_amount);
            var theta = new double[photon_amount];
            var phi = new double[photon_amount];
            var energies = new double[photon_amount];
            var rc = new int[photon_amount];

            
            progressBar1.Minimum = 0;
            progressBar1.Maximum = photon_amount;
            progressBar1.Step = 50000;

            for (int i = 0; i < photon_amount; i++)
            {
                theta[i] = arr[i, 0];
                phi[i] = arr[i, 1];
                rc[i] = -2;
            }

            dt = DateTime.Now;

            for (int i = 0; i < photon_amount; i += 50000)
            {
                LsodarDll.Lsodar1(ref i, ref radvec, ref photon_amount, theta, phi, energies, rc);
                progressBar1.PerformStep();
            }
        

            textBox4.Text = Convert.ToString(DateTime.Now - dt);
    


            //angles

            List<int> Energies = new List<int>();
            List<int> N = new List<int>();

            for (int i = 0; i < photon_amount; i++)
            {
                if (rc[i] == 0)

                    if (theta[i] <= point_of_view + 0.2 && theta[i] >= point_of_view - 0.2)
                        Energies.Add(Convert.ToInt32(energies[i] * 100.0));

            }

            int count1;
            double max = Energies.Max();

            while (Energies.Count > 0)
            {
                eq = Energies[0];
                count1 = Energies.Count;
                Energies.RemoveAll(Equal);
                list.Add(eq, -Energies.Count + count1);
                N.Add(-Energies.Count + count1);

            }
            list.Sort();
            gr.CurveList.Clear();

            

            LineItem myCurve = gr.AddCurve("Spectr", list, Color.Red, SymbolType.Diamond);
            myCurve.Line.IsVisible = true;
            myCurve.Symbol.Fill.Color = Color.Red;
            myCurve.Symbol.Fill.Type = FillType.Solid;
            myCurve.Symbol.Size = 5;
            gr.XAxis.Scale.Min = 0;
            gr.XAxis.Scale.Max = max;
            gr.YAxis.Scale.Min = 0;
            gr.YAxis.Scale.Max = N.Max();
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

            #region Danya's code

            // textBox5.Text = Convert.ToString(DateTime.Now - dt1);


       /*     //Danya's part of code
            list.Clear();
            double rteta, Energy;
            for (int i = 0; i < photon_amount; i++)
            {
                if (rc[i] == 0)
                {
                    rteta = theta[i];
                    Energy = energies[i];
                    if (rteta <= point_of_view + 1.0 && rteta >= point_of_view - 1.0)
                    {
                        int f = (int)(Energy * 1000) - 500;
                        photons[f]++;
                    }
                }
            }


            gr1.CurveList.Clear();
            for (int i = 0; i < photons.Length; i++)
            {
                if (photons[i] != 0) list.Add(i + 500, photons[i]);
            }

            LineItem myCurve1 = gr1.AddCurve("Spectr", list, Color.Blue, SymbolType.Diamond);


            myCurve1.Line.IsVisible = true;


            myCurve1.Symbol.Fill.Color = Color.Blue;


            myCurve1.Symbol.Fill.Type = FillType.Solid;


            myCurve1.Symbol.Size = 5;



            gr1.XAxis.Scale.Min = 700;
            gr1.XAxis.Scale.Max = 2000;


            gr1.YAxis.Scale.Min = 0;
            gr1.YAxis.Scale.Max = photons.Max();


            zedGraphControl3.AxisChange();

            zedGraphControl3.Invalidate();

            /*switch (rcondition)
            {
                case 0:
                    {
                        textBox4.Text = "В бесконечность";
                        break;
                    }
                case 1:
                    {
                        textBox4.Text = "В диск";
                        break;
                    }
                case 2:
                    {
                        textBox4.Text = "В дыру";
                        break;
                    }
                default:
                    break;
            }*/
            /* textBox7.Text = Convert.ToString(Energy);
             textBox5.Text = Convert.ToString(rteta);
             textBox6.Text = Convert.ToString(rphi);
            */
            #endregion
        }


        bool Equal(int energy)
        {
            return energy == eq;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gr = zedGraphControl1.GraphPane;
            dt = new DateTime();
        }

        #region Tests
        private void button2_Click(object sender, EventArgs e)
        {
            double a = CDll1.Rand(0, 10);
            textBox1.Text = Convert.ToString(a);
        }       

        private void button2_Click_1(object sender, EventArgs e)
        {
            RandomSphere random = new RandomSphere(null);
            random.RandomizeForSphere(int.Parse(textBox3.Text) * 1000);
            MessageBox.Show("Операция записи успешно выполнена!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PointPairList list = new PointPairList();
            double radvec = Convert.ToDouble(textBox2.Text);
            double point_of_view = Convert.ToDouble(textBox1.Text);
            int photon_amount = int.Parse(textBox3.Text) * 1000;

            RandomSphere random = new RandomSphere(null);//R

            double[,] arr = random.RandomizeForSphere(photon_amount);//N

            var theta = new double[photon_amount];
            var phi = new double[photon_amount];
            var energies = new double[photon_amount];
            var rc = new int[photon_amount];


            progressBar1.Value = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = photon_amount;
            progressBar1.Step = 1;

            for (int i = 0; i < photon_amount; i++)
            {
                theta[i] = arr[i, 0];
                phi[i] = arr[i, 1];
                rc[i] = -2;
            }


            dt = DateTime.Now;//TIME

            for (int i = 0; i < photon_amount; i++)
            {
                LsodarDll.Lsodar(ref radvec, ref theta[i], ref phi[i], out theta[i], out phi[i], out energies[i], ref rc[i]);

                progressBar1.PerformStep();
            }


            textBox5.Text = Convert.ToString(DateTime.Now - dt);

            List<int> Energies = new List<int>();
            List<int> N = new List<int>();


            for (int i = 0; i < photon_amount; i++)
            {
                if (rc[i] == 0)

                    if (theta[i] <= point_of_view + 0.2 && theta[i] >= point_of_view - 0.2)
                        Energies.Add(Convert.ToInt32(energies[i] * 100.0));

            }


            int count1;
            double max = Energies.Max();

            while (Energies.Count > 0)
            {
                eq = Energies[0];
                count1 = Energies.Count;
                Energies.RemoveAll(Equal);
                list.Add(eq, -Energies.Count + count1);
                N.Add(-Energies.Count + count1);

            }
            list.Sort();
            gr.CurveList.Clear();



            LineItem myCurve = gr.AddCurve("Spectr", list, Color.Red, SymbolType.Diamond);
            myCurve.Line.IsVisible = true;
            myCurve.Symbol.Fill.Color = Color.Red;
            myCurve.Symbol.Fill.Type = FillType.Solid;
            myCurve.Symbol.Size = 5;
            gr.XAxis.Scale.Min = 0;
            gr.XAxis.Scale.Max = max;
            gr.YAxis.Scale.Min = 0;
            gr.YAxis.Scale.Max = N.Max();
            zedGraphControl1.AxisChange();

            zedGraphControl1.Invalidate();
        }

        void ReadRand(double[,] arr)
        {
            StreamReader reader = new StreamReader("E:\\testrand.txt");

            int i = 0;
            byte j = 0;
            while (!reader.EndOfStream)
            {
                arr[i, j] = Convert.ToDouble(reader.ReadLine());

                if (j == 0)
                    j = 1;
                else
                {
                    i++;
                    j = 0;
                }
            }

            reader.Close();
            MessageBox.Show("Чтение завершено!");
        }

        #endregion

        #region Save, Load

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            IFormatter formatter = new BinaryFormatter();
            saveFileDialog1.InitialDirectory = "c:\\";
            saveFileDialog1.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";

            String path;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((path = saveFileDialog1.FileName) != null)
                    {
                        Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                        if (!fail)
                            foreach (ObjectsDB x in GraphicsResults)
                                formatter.Serialize(stream, x);
                        stream.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFormatter formatter = new BinaryFormatter();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            String path;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((path = openFileDialog1.FileName) != null)
                    {
                        Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                        while (stream.Position < stream.Length)
                        {
                            GraphicsResults.Add((ObjectsDB)formatter.Deserialize(stream));
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            double fromr = Convert.ToDouble(fromtextBox.Text);
            double tor = Convert.ToDouble(totextBox.Text);
            double step = Convert.ToDouble(steptextBox.Text);
            int Nphotons = Convert.ToInt16(CountTextBox.Text) * 1000;

            RandomSphere random = new RandomSphere(null);
            double[,] randarr;
            var theta = new double[Nphotons];
            var phi = new double[Nphotons];
            var energies = new double[Nphotons];
            var rc = new int[Nphotons];

            progressBar1.Minimum = 0;
            progressBar1.Maximum = Nphotons;
            progressBar1.Step = 50000;

            dt = DateTime.Now;
            ObjectsDB objectdb = new ObjectsDB();

            for (double i = fromr; i <= tor; i+=step)
            {
                objectdb.R = i;
                objectdb.Energies.Clear();
                objectdb.Angles.Clear();
                randarr = random.RandomizeForSphere(Nphotons);

                for (int j = 0; j < Nphotons; j++)
                {
                    theta[j] = randarr[j, 0];
                    phi[j] = randarr[j, 1];
                    rc[j] = -2;
                }              

                for (int g = 0; g < Nphotons; g += 50000)
                {
                    LsodarDll.Lsodar1(ref g, ref i, ref Nphotons, theta, phi, energies, rc);
                    progressBar1.PerformStep();
                }

                for (int j = 0; j < Nphotons; j++)
                {
                    if (rc[j] == 0)
                    {
                        objectdb.Angle = theta[j];
                        objectdb.Energy = energies[j]*100.0;
                    }
                }

                    progressBar1.Value = 0;
                    GraphicsResults.Add(objectdb);
            }

            textBox4.Text = Convert.ToString(DateTime.Now - dt);
            fail = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            double point_of_view = 30;
        
                List<int> Energies = new List<int>();
                List<int> N = new List<int>();
                
                for (int i = 0; i < GraphicsResults[0].Energies.Count; i++)
                {
                    if (GraphicsResults[0].Angles[i] <= point_of_view + 0.2 && GraphicsResults[0].Angles[i] >= point_of_view - 0.2)
                            Energies.Add(Convert.ToInt32(GraphicsResults[0].Energies[i]));

                }

                int count1;
                double max = Energies.Max();
                PointPairList list = new PointPairList();

                while (Energies.Count > 0)
                {
                    eq = Energies[0];
                    count1 = Energies.Count;
                    Energies.RemoveAll(Equal);
                    list.Add(eq, -Energies.Count + count1);
                    N.Add(-Energies.Count + count1);

                }
                list.Sort();
                // gr.CurveList.Clear();



                LineItem myCurve = gr.AddCurve("Spectr", list, Color.Red, SymbolType.Diamond);
                myCurve.Line.IsVisible = true;
                myCurve.Symbol.Fill.Color = Color.Red;
                myCurve.Symbol.Fill.Type = FillType.Solid;
                myCurve.Symbol.Size = 5;
                gr.XAxis.Scale.Min = 0;
                gr.XAxis.Scale.Max = max;
                gr.YAxis.Scale.Min = 0;
                gr.YAxis.Scale.Max = N.Max();
                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();
            
        }
    }
}
