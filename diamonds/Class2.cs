using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace diamonds
{
    public class AuxiliaryDiamondClass
    {
        Diamond[] diamonds;
        List<Cut> cuts;
        List<Color> colors;
        List<Clarity> clarities;

        public void FileLoader(string path)
        {

        }
        /*Cut cut;
        Color color;
        Clarity clarity;*/
        void AllCut()
        {
            foreach(var d in diamonds)
            {
                if (cuts.IndexOf(d.cut) < 0) cuts.Add(d.cut);
                if (colors.IndexOf(d.color) < 0) colors.Add(d.color);
                if (clarities.IndexOf(d.clarity) < 0) clarities.Add(d.clarity);
            }
            
        }
    }
}
