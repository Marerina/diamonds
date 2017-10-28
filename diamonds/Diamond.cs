using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diamonds
{
    public struct Cut
    {
        public int nom;
        public string name;
    }
    public struct Clarity
    {
        public int nom;
        public string name;
    }
    public struct Color
    {
        public int nom;
        public string name;
    }
    public class Diamond
    {   
        public double carat;
        public Cut cut;
        public Color color;
        public Clarity clarity;
        public double depth;
        public double table;
        public double prise;
        public double x;
        public double y;
        public double z;
    }
}
