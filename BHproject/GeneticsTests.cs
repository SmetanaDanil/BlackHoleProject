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
            
          double p11 = Convert.ToDouble(textBox1.Text);
          double p21 = Convert.ToDouble(textBox2.Text);

          int p1 = (int)(p11 * 100.0);
          int p2 = (int)(p21 * 100.0);

           int point_of_crossing = (int)CDll1.Rand(1, 31);
            //in the bit of crossing
            textBox5.Text = Convert.ToString(point_of_crossing);

            int x = 0, y = 0;

            for (int i = point_of_crossing; i < 31; i++)
                x += (int)Math.Pow(2, i);

            for (int i = 0; i < point_of_crossing; i++)
                y += (int)Math.Pow(2, i);

            double a = ((p1 & x) + (p2 & y))/100.0;
            double b = ((p2 & x) + (p1 & y))/100.0;
            textBox3.Text = Convert.ToString(a);
            textBox4.Text = Convert.ToString(b);
        }
    }
}
