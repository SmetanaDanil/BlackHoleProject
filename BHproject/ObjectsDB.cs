using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BHproject
{
  [Serializable]
    class ObjectsDB
    {
      int n;
      double r;
      List<double> energies;
      List<double> angles;

        public double R
        {
            set
            {
                r = value;
            }
            get
            {
                return r;
            }
        }

        public int N
        {
            set
            {
                n = value;
            }
            get
            {
                return n;
            }
        }

        public double Energy
        {
            set
            {
                energies.Add(value);
            }
        }

        public double Angle
        {
            set
            {
                angles.Add(value);
            }
        }

        public List<double> Energies
        {
            get
            {
                return energies;
            }
        }

        public List<double> Angles
        {
            get
            {
                return angles;
            }
        }

        public ObjectsDB(int n, double r, List<double> energies, List<double> angles)
        {
            this.n = n;
            this.r = r;
            this.energies = energies;
            this.angles = angles;
        }
    }
}
