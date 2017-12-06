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
       public List<Cut> cuts;
        List<Color> colors;
        List<Clarity> clarities;
        decimal MaxNorm;
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
                s = sr.ReadLine();
                string[] diam = s.Split(',');
                Diamond d = new Diamond();
                d.carat = double.Parse(diam[1].Replace(".",","));
                d.depth = double.Parse(diam[5].Replace(".", ","));
                d.table = double.Parse(diam[6].Replace(".", ","));
                d.prise = double.Parse(diam[7].Replace(".", ","));
                d.x = double.Parse(diam[8].Replace(".", ","));
                d.y = double.Parse(diam[9].Replace(".", ","));
                d.z = double.Parse(diam[10].Replace(".", ","));
                int i = IndexCut(diam[2]);
                if (i >= 0) { Cut ct = new Cut(); ct.nom = i; ct.name = diam[2]; d.cut = ct; }
                else { Cut ct = new Cut(); ct.nom = cuts.Count; ct.name = diam[2]; d.cut = ct; cuts.Add(ct); }
                i = IndexColor(diam[3]);
                if (i >= 0) { Color ct = new Color(); ct.nom = i; ct.name = diam[3]; d.color = ct; }
                else { Color ct = new Color(); ct.nom = colors.Count; ct.name = diam[3]; d.color = ct; colors.Add(ct); }
                i = IndexClarity(diam[4]);
                if (i >= 0) { Clarity ct = new Clarity(); ct.nom = i; ct.name = diam[4]; d.clarity = ct; }
                else { Clarity ct = new Clarity(); ct.nom = clarities.Count; ct.name = diam[4]; d.clarity = ct; clarities.Add(ct); }
                diamonds.Add(d);
            }
            MaxNorm = (decimal)MaxValue();
        }
        
        public double MaxValue()
        {
            double mxvl = diamonds[0].prise;
            for (int i = 1; i < diamonds.Count; i++)
                if (mxvl < diamonds[i].prise) mxvl = diamonds[i].prise;
            return mxvl;
        }

        int IndexCut(string s)
        {
            int i = -1;
            foreach (var c in cuts)
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
        // вектор х с номером ind
        public decimal [] CreateX(int ind)
        {
            int n = cuts.Count + clarities.Count + colors.Count;
            decimal[] x = new decimal[6 + n];
            int i = 0;
            x[i] = (decimal)diamonds[ind].carat; i++;
            int tmp = i + cuts.Count;
            for (int j = i; j < tmp; j++)
                x[j] = 0;
            x[diamonds[ind].cut.nom] = 1;
            i = tmp;
            tmp = i + colors.Count;
            for (int j = i; j < tmp; j++)
                x[j] = 0;
            x[diamonds[ind].color.nom] = 1;
            i = tmp;
            tmp = i + clarities.Count;
            for (int j = i; j < tmp; j++)
                x[j] = 0;
            x[diamonds[ind].clarity.nom] = 1;
            i = tmp;
            x[i] = (decimal)diamonds[ind].depth; i++;
            x[i] = (decimal)diamonds[ind].table; i++;
            x[i] = (decimal)diamonds[ind].x; i++;
            x[i] = (decimal)diamonds[ind].y; i++;
            x[i] = (decimal)diamonds[ind].z; i++;
            return x;
        }
        // Цена номера ind
        public decimal[] CreateY(int ind)
        {           
            decimal[] y = new decimal[1];
            y[0] = (decimal)diamonds[ind].prise;
            return y;
        }
        // Запуск нейросети
        // Индекс начала и конца кусочка выборки (не включая верхнюю границу)
        // Count1 - нейронов на скрытом слое, Count2 - нейронов на выходном слое
        // isRandom - рандомно ли берутся веса
        public void Start(int iStart, int iFinish, int Count1, int Count2, bool isRandom)
        {
            for (int i = iStart; i <= iFinish; i++)
            {
                decimal[] x = CreateX(i);
                decimal[] y = CreateY(i);
                NeiroDiamonds n = new NeiroDiamonds(x, y, Count1, Count2, MaxNorm, isRandom);
                decimal Out = NeiroDiamonds.StraightPass(n.x, NeiroDiamonds.W1, NeiroDiamonds.W2);
                NeiroDiamonds.ReversePass(Out, y[0], n.x, NeiroDiamonds.W1, NeiroDiamonds.W2);
            }
        }
    }
}
