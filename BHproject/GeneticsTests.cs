using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BHproject
{
    public partial class GeneticsTests : Form
    {
        public GeneticsTests()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double[] f1 = new double[3];
            double[] f2 = new double[3];
            double[] p1 = new double[] { 1, 0.45, 3 };
            double[] p2 = new double[] { 162, 7.85, 678};

            int number_of_crossing = (int)CDll1.Rand(0, 3);//3=neq

            //inside and outside the bit of crossing
            for (int i = 0; i < number_of_crossing; i++)
            {
                f1[i] = p2[i];
                f2[i] = p1[i];
            }
            for (int i = number_of_crossing + 1; i < 3; i++)
            {
                f1[i] = p1[i];
                f2[i] = p2[i];
            }

            int tempp1 = (int)(p1[number_of_crossing] * 100.0);
            int tempp2 = (int)(p2[number_of_crossing] * 100.0);

            int point_of_crossing = (int)CDll1.Rand(1, 31);
            //in the bit of crossing

            textBox5.Text = Convert.ToString(point_of_crossing);

            int x = 0, y = 0;

            for (int i = point_of_crossing; i < 31; i++)
                x += (int)Math.Pow(2, i);

            for (int i = 0; i < point_of_crossing; i++)
                y += (int)Math.Pow(2, i);

            f1[number_of_crossing] = ((tempp1 & x) + (tempp2 & y)) / 100.0;
            f2[number_of_crossing] = ((tempp2 & x) + (tempp1 & y)) / 100.0;

            foreach (double o in f1)
                textBox3.Text += o + " ";
            foreach (double o in f2)
                textBox4.Text += o + " ";
        }
    }
}
