using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diamonds
{
    class NeiroDiamonds
    {
        Random r;
        // Веса между первым и скрытым слоем
        static double[,] W1;
        // Веса между скрытым и выходным слоем
        static double[,] W2;
        // Вектор входных значений
        double[] x;
        // Вектор значений на скрытом слое
        double[] y;
        // Вектор выходных значений
        double[] z;
        // Вектор решений
        double[] yx;
        // Скорость обучения
        double d;
        // Конструктор (X - Вектор входных значений, Yx - Вектор решений, YLen - Количество нейронов на скрытом слое, ZLen - Количество нейронов на выходном слое)
        public NeiroDiamonds(double[] X, double [] Yx, int YLen, int ZLen)
        {
            y = new double[YLen];
            z = new double[ZLen];
            NewNeiron(X, Yx);
            W1 = new double[x.Length, YLen];
            W2 = new double[YLen, ZLen];
            r = new Random();
            for (int i = 0; i < x.Length; i++)
                for (int j = 0; j < YLen; j++)
                    W1[i, j] = r.NextDouble();
            for (int i = 0; i < YLen; i++)
                for (int j = 0; j < ZLen; j++)
                    W2[i, j] = r.NextDouble();
        }
        public void NewNeiron(double[] X, double[] Yx)
        {
            X.CopyTo(x, 0);
            Yx.CopyTo(yx, 0);
        }
        // Функция активации
        static public double Fa(double x){return 1.0 / (1.0 + Math.Exp(x));}
        // Производная функции активации
        static public double dFa(double x) { return Fa(x) * (1 - Fa(x)); }
        // Функция квадратичной ошибки (Out - получившееся значение ф-и, Y - точное значение ф-и)
        static public double E(double Out, double Y) { return 0.5 * (Out - Y) * (Out - Y); }
        // Производная функции квадратичной ошибки
        static public double dE(double Out, double Y) { return Out - Y; }
        // Функция - сумматор (х - массив значений, W - массив весов, j - номер текущего столбца весов)
        static public double Sum(double[] x, double[,] W, int j)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
                sum += x[i] * W[i, j];
            return sum;
        }
        // Выходное значение (х - массив значений, W - массив весов, j - номер текущего столбца весов)
        static public double Out(double[] x, double[,] W, int j){ return Fa(Sum(x, W, j)); }
        // Прямой проход
        static public double StraightPass(double[] x, double[,] W1, double[,] W2)
        {
            // Результат
            double Z = 0;
            double [] z = new double[W2.GetLength(1)]; ;
            double[] y = new double[W1.GetLength(1)];
            // Проход от x -> y
            for (int j = 0; j < y.Length; j++) { y[j] = Out(x, W1, j); }
            // Проход от y -> z
            for (int j = 0; j < z.Length; j++) { z[j] = Out(y, W2, j); }
            for (int i = 0; i < z.Length; i++) Z += z[i]; 
            return Z / z.Length;
        }
        //static public void ReversePass()
    }
}
