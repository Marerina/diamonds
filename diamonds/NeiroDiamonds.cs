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
        // Скорость обучения
        static decimal n;
        // Веса между первым и скрытым слоем
        static public decimal[,] W1;
        // Веса между скрытым и выходным слоем
        static public decimal[,] W2;
        // Сумма весов между первым и скрытым слоем
        static decimal[,] nW1;
        // Сумма весов между скрытым и выходным слоем
        static decimal[,] nW2;
        // Количество обратных проходов
        static int count;
        // Вектор входных значений
        public decimal[] x;
        // Вектор значений на скрытом слое
        decimal[] y;
        // Вектор выходных значений
        decimal[] z;
        // Вектор решений
        decimal[] yx;
        // Скорость обучения
        decimal d;
        // Нормировка
        static int Norm = 1;

        /*Конструктор (isRandom - Флаг (берем рандомные веса или из усредненной суммы), 
         * Max - максимальное значение для нормировки, 
         * X - Вектор входных значений, 
         * Yx - Вектор решений, 
         * YLen - Количество нейронов на скрытом слое, 
         * ZLen - Количество нейронов на выходном слое)
         */
        public NeiroDiamonds(decimal[] X, decimal[] Yx, int YLen, int ZLen, decimal Max, bool isRandom)
        {
            count = 0;
            y = new decimal[YLen];
            z = new decimal[ZLen];
           
            //Нормировка
            while ((Norm - Max) < 0)
                Norm *= 10;
            
            NewNeiron(X, Yx, Norm);
             Weight(YLen, ZLen, isRandom);
        }
        // Заполнение массивов весов 
        void Weight(int YLen, int ZLen, bool IsRandom)
        {
            W1 = new decimal[x.Length, YLen];
            W2 = new decimal[YLen, ZLen];
            nW1 = new decimal[x.Length, YLen];
            nW2 = new decimal[YLen, ZLen];
            if (IsRandom)
            {
                r = new Random();
                for (int i = 0; i < x.Length; i++)
                    for (int j = 0; j < YLen; j++)
                    {
                        W1[i, j] = (decimal)r.NextDouble();
                        nW1[i, j] = 0;
                    }
                for (int i = 0; i < YLen; i++)
                    for (int j = 0; j < ZLen; j++)
                    {
                        W2[i, j] = (decimal)r.NextDouble();
                        nW2[i, j] = 0;
                    }
            }
            else
            {
                W1 = new decimal[nW1.GetLength(0), nW1.GetLength(1)];
                W2 = new decimal[nW2.GetLength(0), nW2.GetLength(1)];
                for (int i = 0; i < x.Length; i++)
                    for (int j = 0; j < YLen; j++)
                    {
                        W1[i, j] = nW1[i, j];
                        nW1[i, j] = 0;
                    }
                for (int i = 0; i < YLen; i++)
                    for (int j = 0; j < ZLen; j++)
                    {
                        W2[i, j] = nW2[i, j];
                        nW2[i, j] = 0;
                    }

            }
        }

        public void NewNeiron(decimal[] X, decimal[] Yx, int Max)
        {
            x = new decimal[X.Length];
            yx = new decimal[Yx.Length];
            X.CopyTo(x, 0);
            Yx.CopyTo(yx, 0);
            for (int i = 0; i < x.Length; i++)
                x[i] /= Max;
            for (int i = 0; i < yx.Length; i++)
                yx[i] /= Max;
        }
        // Разнормировать
        public void aNorm() { for (int i = 0; i < yx.Length; i++) yx[i] *= Norm; }
        // Функция активации
        static public decimal Fa(decimal x){return (decimal)(1.0 / (1.0 + Math.Exp((double)x)));}
        // Производная функции активации
        static public decimal dFa(decimal x) { return Fa(x) * (1 - Fa(x)); }
        // Функция квадратичной ошибки (Out - получившееся значение ф-и, Y - точное значение ф-и)
        static public decimal E(decimal Out, decimal Y) { return (Out - Y) / 2 * (Out - Y); }
        // Производная функции квадратичной ошибки
        static public decimal dE(decimal Out, decimal Y) { return Out - Y; }
        // Функция - сумматор (х - массив значений, W - массив весов, j - номер текущего столбца весов)
        static public decimal Sum(decimal[] x, decimal[,] W, int j)
        {
            decimal sum = 0;
            for (int i = 0; i < W.GetLength(0); i++)
                sum += x[i] * W[i, j];
            return sum;
        }
        // Выходное значение (х - массив значений, W - массив весов, j - номер текущего столбца весов)
        static public decimal Out(decimal[] x, decimal[,] W, int j){ return Fa(Sum(x, W, j)); }
        static public decimal dOut(decimal[] x, decimal[,] W, int j)
        {
            decimal f = Fa(Sum(x, W, j));
            return f * (1 - f);
        }
        // Погрешность dE/dWij
        static public decimal dEdW(decimal Out, decimal Y, decimal[] x, decimal[,] W, int j) { return dE(Out, Y) * dOut(x, W, j) * Out; }
        // Прямой проход
        static public decimal StraightPass(decimal[] x, decimal[,] W1, decimal[,] W2)
        {
            // Результат
            decimal Z = 0;
            decimal[] z = new decimal[W2.GetLength(1)]; ;
            decimal[] y = new decimal[W1.GetLength(1)];
            // Проход от x -> y
            for (int j = 0; j < y.Length; j++) { y[j] = Out(x, W1, j); }
            // Проход от y -> z
            for (int j = 0; j < z.Length; j++) { z[j] = Out(y, W2, j); }
            for (int i = 0; i < z.Length; i++) Z += z[i]; 
            return Z / z.Length;
        }
        // Обратный проход
        static public void ReversePass(decimal Out, decimal Y, decimal[] x, decimal[,] W1, decimal[,] W2)
        {
            count++;
            Y /= Norm;
            for (int i = 0; i < nW2.GetLength(0); i++)
                for (int j = 0; j < nW2.GetLength(1); j++)
                    nW2[i, j] += W2[i, j] - n * dEdW(Out, Y, x, W2, j);
            for (int i = 0; i < nW1.GetLength(0); i++)
                for (int j = 0; j < nW1.GetLength(1); j++)
                    nW1[i, j] += W1[i, j] - n * dEdW(Out, Y, x, W1, j);
        }
        // Среднее по W1 и W2
        static public void Medium()
        {
            for (int i = 0; i < nW2.GetLength(0); i++)
                for (int j = 0; j < nW2.GetLength(1); j++)
                    nW2[i, j] /= count;
            for (int i = 0; i < nW1.GetLength(0); i++)
                for (int j = 0; j < nW1.GetLength(1); j++)
                    nW1[i, j] /= count;
            // Сбрасываем счетчик
            count = 0;
        }

    }
}
