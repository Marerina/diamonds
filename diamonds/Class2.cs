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
        List<Diamond> diamonds;
        List<Cut> cuts;
        List<Color> colors;
        List<Clarity> clarities;
        public AuxiliaryDiamondClass()
        {
            cuts = new List<Cut>();
            colors = new List<Color>();
            clarities = new List<Clarity>();

        }
        
        public void FileLoader(string path)
        {
            StreamReader sr = new StreamReader(path);
            string s = sr.ReadLine();
            diamonds = new List<Diamond>();
            while (!sr.EndOfStream)
            {
                s =  sr.ReadLine();
                string[] diam = s.Split(',');
                Diamond d = new Diamond();
                d.carat = double.Parse(diam[1]);
                d.depth = double.Parse(diam[5]);
                d.table = double.Parse(diam[6]);
                d.prise = double.Parse(diam[7]);
                d.x = double.Parse(diam[8]);
                d.y = double.Parse(diam[9]);
                d.z = double.Parse(diam[10]);
                int i = IndexCut(diam[2]);
                if (i > 0) { Cut ct = new Cut(); ct.nom = i; ct.name = diam[2]; d.cut = ct; }
                else { Cut ct = new Cut(); ct.nom = cuts.Count; ct.name = diam[2]; d.cut = ct; cuts.Add(ct); }
                i = IndexColor(diam[3]);
                if (i > 0) { Color ct = new Color(); ct.nom = i; ct.name = diam[3]; d.color = ct; }
                else { Color ct = new Color(); ct.nom = colors.Count; ct.name = diam[3]; d.color = ct; colors.Add(ct); }
                i = IndexClarity(diam[4]);
                if (i > 0) { Clarity ct = new Clarity(); ct.nom = i; ct.name = diam[4]; d.clarity = ct; }
                else { Clarity ct = new Clarity(); ct.nom = clarities.Count; ct.name = diam[4]; d.clarity = ct; clarities.Add(ct); }
            }
        }
        /*Cut cut;
        Color color;
        Clarity clarity;*/
        int IndexCut(string s)
        {
            int i = -1;
            foreach(var c in cuts)
            {
                if (c.name == s) { i = c.nom; break; }
            }
            return i;
        }
        int IndexClarity(string s)
        {
            int i = -1;
            foreach (var c in clarities)
            {
                if (c.name == s) { i = c.nom; break; }
            }
            return i;
        }
        int IndexColor(string s)
        {
            int i = -1;
            foreach (var c in colors)
            {
                if (c.name == s) { i = c.nom; break; }
            }
            return i;
        }
    }
}
