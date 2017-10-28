using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diamonds
{
    struct Cut
    {
        int nom;
        string name;
    }
    struct Clarity
    {
        int nom;
        string name;
    }
    struct Color
    {
        int nom;
        string name;
    }
    class Diamond
    {
        double carat;
        Cut cut;
        Color color;
        Clarity clarity;
        double depth;
        double table;
        double prise;
        double x;
        double y;
        double z;
    }
}
