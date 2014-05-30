using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BHproject
{
  [Serializable]
    class ObjectsDB
    {
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

        public ObjectsDB()
        {
            energies = new List<double>();
            angles = new List<double>();
        }
    }
}
