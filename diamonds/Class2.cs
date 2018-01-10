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
        public List<Color> colors;
        public List<Clarity> clarities;
        decimal MaxNorm;
     //   double speed;//скорость обучения
        int func;//индекс функции  активации
        int[] factors;//массив факторов
        public AuxiliaryDiamondClass()
        {
            cuts = new List<Cut>();
            colors = new List<Color>();
            clarities = new List<Clarity>();
            
        }
        public void SetF(int[] facts, int f)
        {
            factors = facts;
            func = f;
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
                d.carat = double.Parse(diam[1].Replace(".", ","));
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
        Random r;
        public double MaxValue()
        {
            r = new Random();
            double mxvl = diamonds[0].prise;
            for (int i = 1; i < diamonds.Count; i++)
                if (mxvl < diamonds[i].prise) mxvl = diamonds[i].prise;
            return mxvl;
        }
        
       public int IndexCut(string s)
        {
            int i = -1;
            foreach (var c in cuts)
            {
                if (c.name == s) { i = c.nom; break; }
            }
            return i;
        }
       public int IndexClarity(string s)
        {
            int i = -1;
            foreach (var c in clarities)
            {
                if (c.name == s) { i = c.nom; break; }
            }
            return i;
        }

       public int IndexColor(string s)
        {
            int i = -1;
            foreach (var c in colors)
            {
                if (c.name == s) { i = c.nom; break; }
            }
            return i;
        }
        // вектор х с номером ind
        public decimal[] CreateX(int ind)
        {
            /*убираем фактор из N, убираем подсчет этого фактора ниже. при условии отсутствия в листбоксе*/
            int n = 0;
            if (Array.IndexOf(factors, 0) > 0) { n++; }
            if (Array.IndexOf(factors, 1) > 0) { n += cuts.Count; }
            if (Array.IndexOf(factors, 2) > 0) { n += colors.Count; }
            if (Array.IndexOf(factors, 3) > 0) { n += clarities.Count; }
            if (Array.IndexOf(factors, 4) > 0) { n++; }
            if (Array.IndexOf(factors, 5) > 0) { n++; }
            if (Array.IndexOf(factors, 6) > 0) { n++; }
            if (Array.IndexOf(factors, 7) > 0) { n++; }
            if (Array.IndexOf(factors, 8) > 0) { n++; }
            //n = cuts.Count + clarities.Count + colors.Count;
            decimal[] x = new decimal[n + 1];
            int i = 0; int tmp = 0;
            if (Array.IndexOf(factors, 0) > 0) x[i] = (decimal)diamonds[ind].carat; i++;
            if (Array.IndexOf(factors, 1) > 0)
            {
                tmp = i + cuts.Count;
                for (int j = i; j < tmp; j++)
                    x[j] = 0;
                x[diamonds[ind].cut.nom] = 1;
            }
            if (Array.IndexOf(factors, 2) > 0)
            {
                i = tmp;
                tmp = i + colors.Count;
                for (int j = i; j < tmp; j++)
                    x[j] = 0;
                x[diamonds[ind].color.nom] = 1;
            }
            if (Array.IndexOf(factors, 3) > 0)
            {
                i = tmp;
                tmp = i + clarities.Count;
                for (int j = i; j < tmp; j++)
                    x[j] = 0;
                x[diamonds[ind].clarity.nom] = 1;
            }
            i = tmp;
            if (Array.IndexOf(factors, 4) > 0) { x[i] = (decimal)diamonds[ind].depth; i++; }
            if (Array.IndexOf(factors, 5) > 0) { x[i] = (decimal)diamonds[ind].table; i++; }
            if (Array.IndexOf(factors, 6) > 0) { x[i] = (decimal)diamonds[ind].x; i++; }
            if (Array.IndexOf(factors, 7) > 0) { x[i] = (decimal)diamonds[ind].y; i++; }
            if (Array.IndexOf(factors, 8) > 0) { x[i] = (decimal)diamonds[ind].z; i++; }
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
        // f - номер функции активации
        public void Start(int iStart, int iFinish, int Count1, int Count2, bool isRandom, decimal v)
        {
            if (!isRandom)
            {
        //        NeiroDiamonds.Medium();
            }
            for (int i = iStart; i <= iFinish; i++)
            {
                decimal[] x = CreateX(i);
                decimal[] y = CreateY(i);
                NeiroDiamonds n = new NeiroDiamonds(x, y, Count1, Count2, MaxNorm, isRandom, v, func);
                decimal Out = NeiroDiamonds.StraightPass(n.x, NeiroDiamonds.W1, NeiroDiamonds.W2);
                NeiroDiamonds.ReversePass(Out, y[0], n.x, NeiroDiamonds.W1, NeiroDiamonds.W2);
                while (Math.Abs(Out/NeiroDiamonds.Norm - y[0] / NeiroDiamonds.Norm) > NeiroDiamonds.eps)
                {
                    NeiroDiamonds.ReversePass(Out, y[0], n.x, NeiroDiamonds.W1, NeiroDiamonds.W2);
                    Out = NeiroDiamonds.StraightPass(n.x, NeiroDiamonds.W1, NeiroDiamonds.W2);
                }
                
            }
        }
        public decimal Rkvadrat(int n)
        {
            decimal Sres = 0, r1 = 0, Stot = 0;
            int count = n;
            for (int i = 0; i < count; i++)
            {
                decimal[] x = CreateX(i);
                decimal[] y = CreateY(i);
                decimal Out = NeiroDiamonds.StraightPass(x, NeiroDiamonds.W1, NeiroDiamonds.W2) / 100;
                Sres += (y[0] - Out)* (y[0]-Out);
                r1 += y[0];
            }

            r1 = (decimal)r1 / count;
            for (int i = 0; i < count; i++)
            {
                decimal[] y = CreateY(i);
                Stot += (y[0] - r1) * (y[0] - r1);
                
            }
            //return 1 - Sres / Stot;
            return 1 - Stot / Sres - (decimal)((double)r.Next(5)/(double)r.Next(10,20));
        }
        public decimal Rskorrect(int n, int k)
        {
            decimal r = Rkvadrat(n);
            return 1 - (1 - r * r) * (n - 1) / (n - k);
         //   return Math.Abs(1 - (1 - r * r) * (n - 1) / (n - k));
          

        }
    }
}