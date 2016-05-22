using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenAlg
{
    public class gaTSP
    {

        private int[][] AdaptationMatrix;
        private int[][] Parents;
        private int[][] Children;
        private int[][] Mutants;
        private int nu;
        private int L;
        private double DeltaMu;
        private Random rand;

        public int sum;
        public static int min = 0;
        public static int max = 0;

        //	public gaTSP(int[][] Coordinates, int nu) {

        //	}

        public gaTSP(int[][] OriginalAdaptation, int nu)
        {
            this.L = OriginalAdaptation.Length;
            this.AdaptationMatrix = new int[L][];
            this.nu = nu;
            this.Parents = new int[this.nu][];
            //this.Children = new int[this.nu, this.L];
            this.Mutants = new int[this.nu][];
            this.DeltaMu = 0.0;
            this.rand = new Random();


            if (OriginalAdaptation.Length == OriginalAdaptation[0].Length)
            {
                this.AdaptationMatrix = OriginalAdaptation;
            }
            else
            {
                if (OriginalAdaptation[0].Length == 2)
                {
                    for (int i = 0; i < this.L; i++)
                    {
                        for (int j = 0; j < this.L; j++)
                        {
                            //if(i == j) {
                            //this.AdaptationMatrix[i][j] = 0;
                            //continue;
                            //}
                            if (j < i)
                            {
                                this.AdaptationMatrix[i][j] = this.AdaptationMatrix[j][i];
                                continue;
                            }
                            double a = OriginalAdaptation[i][0] - OriginalAdaptation[j][0];
                            double b = OriginalAdaptation[i][1] - OriginalAdaptation[j][1];
                            a = Math.Abs(a);
                            b = Math.Abs(b);
                            a *= a;
                            b *= b;
                            double c = a + b;
                            c = Math.Sqrt(c);
                            this.AdaptationMatrix[i][j] = (int)c;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Объект создан неверно.\n"
                            + "Матрица приспособленности должна быть квадратной.");
                }

            }

            for (int i = 0; i < L; i++)
                if (AdaptationMatrix[i][i] == 0) AdaptationMatrix[i][i] = Int32.MaxValue;

            for (int i = 0; i < nu; i++)
                this.Parents[i] = greedCode(L); //мб стоит вынести отдельную функцию создания первых родителей

        }

        private int[] greedCode(int L)
        {
            int[] arr = new int[L];
            int[] cities = new int[L];
            for (int i = 0; i < L; i++)
            {
                cities[i] = i;
            }
            arr[0] = (int)Math.Floor(rand.NextDouble() * L);
            cities[arr[0]] = -1;
            int MAXVAR = (int)Math.Floor(L * 0.1) + 1; // 10%
            
            for (int i = 1; i < arr.Length; i++)
            {
                int[] vars = new int[MAXVAR];
                vars = fillMinusOne(vars);
                for (int j = 0; j < vars.Length; j++)
                {
                    for (int k = 0; k < cities.Length; k++)
                    {
                        if (!contain(vars, k) && cities[k] > -1)
                        {
                            vars[j] = k;
                            break;
                        }
                    }

                    for (int k = 0; k < cities.Length; k++)
                    {
                        if (!contain(vars, k) && cities[k] > -1)
                        {
                            if (AdaptationMatrix[arr[i - 1]][k] < AdaptationMatrix[arr[i - 1]][vars[j]])
                            {
                                vars[j] = k;
                            }
                        }
                    }
                }
                do {
                    arr[i] = vars[(int)Math.Floor(rand.NextDouble() * vars.Length)];
                } while(arr[i] < 0);
                cities[arr[i]] = -1;
            }

            return arr;
        }

        private int[] fillMinusOne(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = -1;
            }
            return arr;
        }

        private bool contain(int[] arr, int a)
        {
            foreach (int x in arr)
                if (x == a) return true;
            return false;
        }

        private int[] greedRandCode(int L)
        {
            int MAXVAR = (int)Math.Floor(L * 0.1) + 1; // 10%
            int[] vars = new int[MAXVAR];
            int[] arr = new int[L];
            int[] cities = new int[L];
            int temp;
            int smth = L - 1;
            int best = L - 1;
            for (int i = 0; i < L; i++)
            {
                cities[i] = i;
            }
            arr[0] = (int)Math.Floor(rand.NextDouble() * L);
            cities[arr[0]] = Int32.MaxValue;
            for (int i = 1; i < L; i++)
            {
                for (int j = 0; j < MAXVAR; j++)
                {
                    do
                    {
                        temp = (int)Math.Floor(rand.NextDouble() * L);
                    }
                    while (cities[temp] != temp /*&& arr[i - 1] == temp*/);
                    vars[j] = temp;

                    for (int k = 0; k < j; k++) ;

                    //if (AdaptationMatrix[arr[i - 1]][temp] < AdaptationMatrix[arr[i - 1]][smth])

                }
                best = arr[i - 1];
                for (int j = 0; j < MAXVAR; j++)
                {
                    if (AdaptationMatrix[arr[i - 1]][vars[j]] < AdaptationMatrix[arr[i - 1]][best])
                        best = vars[j];
                }
                cities[best] = Int32.MaxValue;
                arr[i] = best;
            }
            return arr;
        }

        private int[] randomCode(int L)
        {
            int[] arr = new int[L];
            bool again;
            for (int i = 0; i < L; i++)
            {
                do
                {
                    again = true;
                    arr[i] = (int)Math.Floor(rand.NextDouble() * L);
                    for (int j = 0; j < i; j++)
                    {
                        if (arr[i] == arr[j]) again = false;
                    }
                } while (!again);
            }
            return arr;
        }

        private int[][] summArr(int[][] arr1, int[][] arr2)
        {
            int j = 0;
            int D_arr = arr1.Length + arr2.Length;
            int[][] resArr = new int[D_arr][];//[L];
            for (int i = 0; i < arr1.Length; i++)
            {
                resArr[i] = arr1[i];
            }
            for (int i = arr1.Length; i < D_arr; i++)
            {
                resArr[i] = arr2[j];
                j++;
            }
            return resArr;
        }

        private double deltaMu(int[][] mp, int[][] adaptationMatrix2)
        {
            double[] DeltaMu = new double[mp.Length];
            int m1 = 0;
            int m2 = 0;
            for (int i = 0; i < mp.Length; i++)
            {
                for (int j = 0; j < mp[0].Length - 1; j++)
                {
                    m1 = mp[i][j];
                    m2 = mp[i][j + 1];
                    DeltaMu[i] += adaptationMatrix2[m1][m2];
                }
                //int _j = mp[0].Length - 1;
                //m1 = mp[i][_j];
                m1 = mp[i][0];
                DeltaMu[i] += adaptationMatrix2[m2][m1];
            }
            double DeltaMuEND = 0;
            double DMuMAX = 0;
            double DMuMIN = DeltaMu[0];
            for (int i = 0; i < DeltaMu.Length; i++)
            {
                if (DeltaMu[i] > DMuMAX) DMuMAX = DeltaMu[i];
                if (DeltaMu[i] < DMuMIN) DMuMIN = DeltaMu[i];
            }
            DeltaMuEND = (DMuMAX - DMuMIN) / 2.0; //PROVERIT'!!!
            return DeltaMuEND;
        }

        private int Mu(int[][] adaptationMatrix2, int[] arr)
        {//вычисление функции приспособленности
            int mu = 0;
            for (int i = 0; i < arr.Length - 1; i++)
                mu += adaptationMatrix2[arr[i]][arr[i + 1]];
            mu += adaptationMatrix2[arr[arr.Length - 1]][arr[0]];
            return mu;
        }

        private int[] mutOneDot(int[] ch, int L)
        {
            int point = (int)Math.Floor(rand.NextDouble() * L);
            int point2 = 0;
            if (point != 0)
                if (point != (L - 1))
                    if (rand.NextDouble() <= 0.5)
                        point2 = point - 1;
                    else point2 = point + 1;
            if (point == 0)
                point2 = 1;
            if (point == (L - 1))
                point2 = (L - 1) - 1;
            int empty = ch[point];
            ch[point] = ch[point2];
            ch[point2] = empty;
            return ch;
        }

        private int[] mutTwoDot(int[] ch, int L)
        {
            int point;
            int point2;
            do
            {
                point = (int)Math.Floor(rand.NextDouble() * L);
                point2 = (int)Math.Floor(rand.NextDouble() * L);
            } while (point == point2);
            int empty = ch[point];
            ch[point] = ch[point2];
            ch[point2] = empty;
            return ch;
        }

        private int[] randomMutant(int mut)
        { //сделать отдельную функцию мутантов
            int[] child = Children[(int)Math.Floor(rand.NextDouble() * Children.Length)];
            if (rand.NextDouble() <= 0.1)
            {
                switch (mut)
                {
                    case 1: child = mutOneDot(child, L);
                        break;

                    case 2: child = mutTwoDot(child, L);
                        break;

                    default: return null;
                }
            }
            return child;
        }

        private int[] copyCycle(int[] Sp, int k, int[] C, int[] Sc)
        {
            for (int i = 0; i < Sp.Length; i++)
                if (C[i] == k)
                    Sc[i] = Sp[i];
            return Sc;
        }

        private int[] crossCycle(int[] Sp1, int[] Sp2)
        {
            int[] C = new int[Sp1.Length];
            int k = 0;
            int[] Sc = new int[Sp1.Length];
            int i = 0;
            int j;
            bool End;
            do
            {
                k++;
                do
                {
                    j = (int)Math.Floor(rand.NextDouble() * (Sp1.Length));
                } while (C[j] != 0);
                while (C[j] == 0)
                {
                    C[j] = k;
                    i = 0;
                    while (Sp1[i] != Sp2[j])
                        i++;
                    j = i;
                }
                Sc = copyCycle(Sp1, k, C, Sc);
                End = true;
                for (int end = 0; end < Sp1.Length; end++)
                {
                    if (C[end] == 0)
                        End = false;
                }
            } while (!End);
            return Sc;
        }

        private int[] crossArithmetic(int[] mom, int[] dad)
        {
            int[] child = new int[mom.Length];
            double[] embryo = new double[mom.Length];
            double[] embryo_sort = new double[mom.Length];
            int[] embryo_num = new int[mom.Length];
            double alpha = rand.NextDouble();

            for (int i = 0; i < embryo.Length; i++)
            {
                embryo[i] = alpha * mom[i] + (1.0 - alpha) * dad[i];
                embryo_sort[i] = embryo[i];
            }
            for (int j = 0; j < embryo_sort.Length; j++)
            {
                for (int i = 0; i < embryo_sort.Length - 1; i++)
                {
                    if (embryo_sort[i] > embryo_sort[i + 1])
                    {
                        double temp = embryo_sort[i];
                        embryo_sort[i] = embryo_sort[i + 1];
                        embryo_sort[i + 1] = temp;
                    }
                }
                embryo_num[j] = j;
            }
            for (int i = 0; i < child.Length; i++)
            {
                for (int j = 0; j < embryo.Length; j++)
                {
                    if (embryo[i] == embryo_sort[j])
                    {
                        child[i] = embryo_num[j];
                    }
                }
            }

            return child;
        }

        int[] crossBinary(int[] mom, int[] dad)
        {
            int[] child = new int[mom.Length];
            int[] embryo_num = new int[mom.Length];
            for (int i = 0; i < child.Length; i++)
            {
                if (rand.NextDouble() <= 0.5)
                {
                    child[i] = mom[i];
                }
                else
                {
                    child[i] = dad[i];
                }
                embryo_num[i] = i;
            }
            for (int i = 0; i < child.Length; i++)
            {
                embryo_num[child[i]] = -1;
            }
            for (int i = 0; i < child.Length - 1; i++)
            {
                for (int j = i + 1; j < child.Length; j++)
                {
                    if (child[i] == child[j])
                    {
                        for (int k = 0; k < child.Length; k++)
                        {
                            if (embryo_num[k] != -1)
                            {
                                child[j] = embryo_num[k];
                                embryo_num[k] = -1;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            return child;
        }

        private bool associatePlus(double mu1, double mu2, double DeltaMu)
        {//мю1 и мю2 - ф-ии приспособленности //ассоциативное положительное скрещивание
            if (Math.Abs(mu1 - mu2) <= DeltaMu * 2)
                return true;
            else
                return false;
        }

        private bool associateMinus(double mu1, double mu2, double DeltaMu)
        {//мю1 и мю2 - ф-ии приспособленности //ассоциативное отрицательное скрещивание
            if (Math.Abs(mu1 - mu2) >= DeltaMu / 2)
                return true;
            else
                return false;
        }

        private int[][] notVozr(int[][] Reprod)
        {//расстановка в порядке монотонного невозрастания
            int[][] R = Reprod;
            for (int j = 0; j < R.Length - 1; j++)
            {
                for (int i = 0; i < R.Length - 1; i++)
                {
                    //int[] m1 = R[i];
                    //int[] m2 = R[i + 1];
                    //	            if (i == 8)
                    //	                d = true;
                    if (Mu(AdaptationMatrix, R[i]) > Mu(AdaptationMatrix, R[i + 1]))
                    {
                        int[] temp = R[i];
                        R[i] = R[i + 1];
                        R[i + 1] = temp;
                        //R[i] = m2;
                        //R[i + 1] = m1;
                        //	                d = true;
                    }
                }
            }
            return R;
        }

        private int[][] lineRangSel(int[][] reprod, int nu)
        {
            double np = 2 - rand.NextDouble();
            double nm = 2 - np;
            reprod = notVozr(reprod);
            int[][] R = new int[nu][];
            int k = 0;
            for (int i = 0; i < reprod.Length; i++)
                if (k < nu)
                    if ((1 - rand.NextDouble()) < (nm + (np - nm) * (i) / (reprod.Length - 1)))
                    {
                        R[k] = reprod[i];
                        k++;
                    }
            return R;
        }

        private int[][] betaTournSel(int[][] reprod, int nu)
        {
            int[][] R = new int[nu][];
            int r;
            int r1;
            int r2;
            int r3;
            for (int i = 0; i < nu; i++)
            {
                r1 = (int)Math.Floor(rand.NextDouble() * reprod.Length);
                do
                {
                    r2 = (int)Math.Floor(rand.NextDouble() * reprod.Length);
                    r3 = (int)Math.Floor(rand.NextDouble() * reprod.Length);
                } while (r2 == r1 || r3 == r1 || r2 == r3);
                if (Mu(AdaptationMatrix, reprod[r1]) > Mu(AdaptationMatrix, reprod[r2]))
                    if (Mu(AdaptationMatrix, reprod[r2]) > Mu(AdaptationMatrix, reprod[r3]))
                        r = r3;
                    else r = r2;
                else
                    if (Mu(AdaptationMatrix, reprod[r1]) > Mu(AdaptationMatrix, reprod[r3]))
                        r = r3;
                    else r = r1;
                R[i] = reprod[r];
            }
            return R;
        }

        /*	void realize(bool st, bool sel, bool mut1) {
                double[][] matrix = {{0.00, 3.16, 13.60, 8.49, 1.00, 13.00, 5.83, 13.04, 13.00, 2.00, 12.53, 12.37, 13.34, 13.15, 5.10}, {3.16, 0.00, 13.89, 5.83, 2.24, 12.37, 2.83, 12.17, 13.60, 5.10, 13.45, 11.00, 13.42, 13.00, 8.00}, {13.60, 13.89, 0.00, 12.21, 13.93, 4.00, 13.45, 5.00, 1.41, 13.15, 2.83, 7.07, 1.00, 2.00, 12.04}, {8.49, 5.83, 12.21, 0.00, 7.81, 9.22, 3.16, 8.60, 12.53, 10.00, 13.00, 6.71, 11.40, 10.63, 12.08}, {1.00, 2.24, 13.93, 7.81, 0.00, 13.04, 5.00, 13.00, 13.42, 3.00, 13.04, 12.17, 13.60, 13.34, 6.08}, {13.00, 12.37, 4.00, 9.22, 13.04, 0.00, 11.18, 1.00, 5.10, 13.15, 6.32, 3.16, 3.00, 2.00, 13.00}, {5.83, 2.83, 13.45, 3.16, 5.00, 11.18, 0.00, 10.77, 13.45, 7.62, 13.60, 9.22, 12.81, 12.21, 10.20}, {13.04, 12.17, 5.00, 8.60, 13.00, 1.00, 10.77, 0.00, 6.08, 13.34, 7.28, 2.24, 4.00, 3.00, 13.42}, {13.00, 13.60, 1.41, 12.53, 13.42, 5.10, 13.45, 6.08, 0.00, 12.37, 1.41, 8.00, 2.24, 3.16, 11.00}, {2.00, 5.10, 13.15, 10.00, 3.00, 13.15, 7.62, 13.34, 12.37, 0.00, 11.70, 13.00, 13.04, 13.00, 3.16}, {12.53, 13.45, 2.83, 13.00, 13.04, 6.32, 13.60, 7.28, 1.41, 11.70, 0.00, 9.06, 3.61, 4.47, 10.05}, {12.37, 11.00, 7.07, 6.71, 12.17, 3.16, 9.22, 2.24, 8.00, 13.00, 9.06, 0.00, 6.08, 5.10, 13.60}, {13.34, 13.42, 1.00, 11.40, 13.60, 3.00, 12.81, 4.00, 2.24, 13.04, 3.61, 6.08, 0.00, 1.00, 12.17}, {13.15, 13.00, 2.00, 10.63, 13.34, 2.00, 12.21, 3.00, 3.16, 13.00, 4.47, 5.10, 1.00, 0.00, 12.37}, {5.10, 8.00, 12.04, 12.08, 6.08, 13.00, 10.20, 13.42, 11.00, 3.16, 10.05, 13.60, 12.17, 12.37, 0.00}};
                int t = 0; 
                int nu = 50;
                int L = 15;
                int[][] P = {{1, 6, 11, 2, 7, 12, 3, 8, 13, 4, 9, 14, 5, 10, 15, 6, 11, 1}, {2, 7, 12, 3, 8, 13, 4, 9, 14, 5, 10, 15, 6, 11, 1, 7, 12, 2}, {3, 8, 13, 4, 9, 14, 5, 10, 15, 6, 11, 1, 7, 12, 2, 8, 13, 3}, {4, 9, 14, 5, 10, 15, 6, 11, 1, 7, 12, 2, 8, 13, 3, 9, 14, 4}, {5, 10, 15, 6, 11, 1, 7, 12, 2, 8, 13, 3, 9, 14, 4, 10, 15, 5}, {6, 11, 1, 7, 12, 2, 8, 13, 3, 9, 14, 4, 10, 15, 5, 11, 1, 6}, {7, 12, 2, 8, 13, 3, 9, 14, 4, 10, 15, 5, 11, 1, 6, 12, 2, 7}, {8, 13, 3, 9, 14, 4, 10, 15, 5, 11, 1, 6, 12, 2, 7, 13, 3, 8}, {9, 14, 4, 10, 15, 5, 11, 1, 6, 12, 2, 7, 13, 3, 8, 14, 4, 9}, {10, 15, 5, 11, 1, 6, 12, 2, 7, 13, 3, 8, 14, 4, 9, 15, 5, 10}, {11, 1, 6, 12, 2, 7, 13, 3, 8, 14, 4, 9, 15, 5, 10, 1, 6, 11}, {12, 2, 7, 13, 3, 8, 14, 4, 9, 15, 5, 10, 1, 6, 11, 2, 7, 12}, {13, 3, 8, 14, 4, 9, 15, 5, 10, 1, 6, 11, 2, 7, 12, 3, 8, 13}, {14, 4, 9, 15, 5, 10, 1, 6, 11, 2, 7, 12, 3, 8, 13, 4, 9, 14}, {15, 5, 10, 1, 6, 11, 2, 7, 12, 3, 8, 13, 4, 9, 14, 5, 10, 15}, {1, 6, 11, 2, 7, 12, 3, 8, 13, 4, 9, 14, 5, 10, 15, 6, 11, 1}, {2, 7, 12, 3, 8, 13, 4, 9, 14, 5, 10, 15, 6, 11, 1, 7, 12, 2}, {3, 8, 13, 4, 9, 14, 5, 10, 15, 6, 11, 1, 7, 12, 2, 8, 13, 3}, {4, 9, 14, 5, 10, 15, 6, 11, 1, 7, 12, 2, 8, 13, 3, 9, 14, 4}, {5, 10, 15, 6, 11, 1, 7, 12, 2, 8, 13, 3, 9, 14, 4, 10, 15, 5}, {6, 11, 1, 7, 12, 2, 8, 13, 3, 9, 14, 4, 10, 15, 5, 11, 1, 6}, {7, 12, 2, 8, 13, 3, 9, 14, 4, 10, 15, 5, 11, 1, 6, 12, 2, 7}, {8, 13, 3, 9, 14, 4, 10, 15, 5, 11, 1, 6, 12, 2, 7, 13, 3, 8}, {9, 14, 4, 10, 15, 5, 11, 1, 6, 12, 2, 7, 13, 3, 8, 14, 4, 9}, {10, 15, 5, 11, 1, 6, 12, 2, 7, 13, 3, 8, 14, 4, 9, 15, 5, 10}, {11, 1, 6, 12, 2, 7, 13, 3, 8, 14, 4, 9, 15, 5, 10, 1, 6, 11}, {12, 2, 7, 13, 3, 8, 14, 4, 9, 15, 5, 10, 1, 6, 11, 2, 7, 12}, {13, 3, 8, 14, 4, 9, 15, 5, 10, 1, 6, 11, 2, 7, 12, 3, 8, 13}, {14, 4, 9, 15, 5, 10, 1, 6, 11, 2, 7, 12, 3, 8, 13, 4, 9, 14}, {15, 5, 10, 1, 6, 11, 2, 7, 12, 3, 8, 13, 4, 9, 14, 5, 10, 15}, {1, 6, 11, 2, 7, 12, 3, 8, 13, 4, 9, 14, 5, 10, 15, 6, 11, 1}, {2, 7, 12, 3, 8, 13, 4, 9, 14, 5, 10, 15, 6, 11, 1, 7, 12, 2}, {3, 8, 13, 4, 9, 14, 5, 10, 15, 6, 11, 1, 7, 12, 2, 8, 13, 3}, {4, 9, 14, 5, 10, 15, 6, 11, 1, 7, 12, 2, 8, 13, 3, 9, 14, 4}, {5, 10, 15, 6, 11, 1, 7, 12, 2, 8, 13, 3, 9, 14, 4, 10, 15, 5}, {6, 11, 1, 7, 12, 2, 8, 13, 3, 9, 14, 4, 10, 15, 5, 11, 1, 6}, {7, 12, 2, 8, 13, 3, 9, 14, 4, 10, 15, 5, 11, 1, 6, 12, 2, 7}, {8, 13, 3, 9, 14, 4, 10, 15, 5, 11, 1, 6, 12, 2, 7, 13, 3, 8}, {9, 14, 4, 10, 15, 5, 11, 1, 6, 12, 2, 7, 13, 3, 8, 14, 4, 9}, {10, 15, 5, 11, 1, 6, 12, 2, 7, 13, 3, 8, 14, 4, 9, 15, 5, 10}, {11, 1, 6, 12, 2, 7, 13, 3, 8, 14, 4, 9, 15, 5, 10, 1, 6, 11}, {12, 2, 7, 13, 3, 8, 14, 4, 9, 15, 5, 10, 1, 6, 11, 2, 7, 12}, {13, 3, 8, 14, 4, 9, 15, 5, 10, 1, 6, 11, 2, 7, 12, 3, 8, 13}, {14, 4, 9, 15, 5, 10, 1, 6, 11, 2, 7, 12, 3, 8, 13, 4, 9, 14}, {15, 5, 10, 1, 6, 11, 2, 7, 12, 3, 8, 13, 4, 9, 14, 5, 10, 15}, {1, 6, 11, 2, 7, 12, 3, 8, 13, 4, 9, 14, 5, 10, 15, 6, 11, 1}, {2, 7, 12, 3, 8, 13, 4, 9, 14, 5, 10, 15, 6, 11, 1, 7, 12, 2}, {3, 8, 13, 4, 9, 14, 5, 10, 15, 6, 11, 1, 7, 12, 2, 8, 13, 3}, {4, 9, 14, 5, 10, 15, 6, 11, 1, 7, 12, 2, 8, 13, 3, 9, 14, 4}, {5, 10, 15, 6, 11, 1, 7, 12, 2, 8, 13, 3, 9, 14, 4, 10, 15, 5}}; //стандартные начальные популяции
                int[][] CH = {};
                int[][] M = null;
		
                if(st)
                    for(int i = 0; i < nu; i++)
                        P[i] = randomCode(L);
                //else 
                //	for(int i = 0; i < nu; i++)
                //		P[i] = blizhCode(L);
		
                for(; t < nu; t++) {
			
                    double DMu = deltaMu(P, matrix);
			
                    for(int i = 0; i < nu;) {
                        int n1 = (int) Math.Floor(rand.NextDouble() * nu);
                        int n2;
                        do {
                            n2 = (int) Math.Floor(rand.NextDouble() * nu);
                        } while(n1 == n2);
				
                        int N1 = Mu(matrix, P[n1]);
                        int N2 = Mu(matrix, P[n2]);
				
                        if(associatePlus(N1, N2, DMu)) {
                            CH[i] = crossCycle(P[n1], P[n2]);
                            i++;
                        }
                    }
			
                    //for(int i = 0; i < 0; i++) 
                        //M[i] = randomMutant(CH, L, mut1);
			
                    int[][] R = summArr(M, CH);
                    R = summArr(R, P);
			
                    //if(sel) R = lineRangSel(R, matrix, nu);
                    //else R = betaTournSel(R, matrix, nu);
			
                    P = R;
                }
		
                P = notVozr(P);
                String t1 = P[P.Length - 1] + ", с приспособленностью " + Mu(matrix, P[P.Length - 1]);
            }*/

        private int findNameMaxAllel(int[] arr)
        {
            int max = 0;
            int imax = 0;

            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (AdaptationMatrix[arr[i]][arr[i + 1]] > max)
                {
                    max = AdaptationMatrix[arr[i]][arr[i + 1]];
                    imax = i + 1;
                }
            }
            return imax;
        }

        public void crossover(int CrossingType)
        {
            DeltaMu = deltaMu(Parents, AdaptationMatrix);
            this.Children = new int[this.nu][];//[this.L];

            for (int i = 0; i < nu; )
            {
                int n1 = (int)Math.Floor(rand.NextDouble() * nu);
                int n2;

                do
                {
                    n2 = (int)Math.Floor(rand.NextDouble() * nu);
                } while (n1 == n2);

                int N1 = Mu(AdaptationMatrix, Parents[n1]);
                int N2 = Mu(AdaptationMatrix, Parents[n2]);

                int[] child = new int[L];

                switch (CrossingType)
                {
                    case 1:
                        if (associatePlus(N1, N2, DeltaMu))
                        {
                            child = crossCycle(Parents[n1], Parents[n2]);
                        }
                        break;
                    case 2:
                        if (associatePlus(N1, N2, DeltaMu))
                        {
                            child = crossArithmetic(Parents[n1], Parents[n2]);
                        }
                        break;
                    case 3:
                        if (associatePlus(N1, N2, DeltaMu))
                        {
                            child = crossBinary(Parents[n1], Parents[n2]);
                        }
                        break;
                }

                int[] anotherChild = new int[L];
                int k = 0;
                for (int j = findNameMaxAllel(child); j < child.Length; j++)
                {
                    anotherChild[k++] = child[j];
                }
                for (int j = 0; j < findNameMaxAllel(child); j++)
                {
                    anotherChild[k++] = child[j];
                }
                if (Mu(AdaptationMatrix, anotherChild) < Mu(AdaptationMatrix, child))
                {
                    Children[i] = anotherChild;
                }
                else
                {
                    Children[i] = child;
                }
                if (Children[i][0] != Children[i][1])
                {
                    i++;
                }
            }

            /*int k = 0;
            for(int i = 0; i < nu; i++) {
                for(int j = 0; j < nu; j++) {
                    if(associatePlus(Mu(AdaptationMatrix, Parents[i]), Mu(AdaptationMatrix, Parents[j]), DeltaMu)) {
                        Children[k] = crossCycle(Parents[i], Parents[j]);
                        k++;
                        Children[k] = crossCycle(Parents[j], Parents[i]);
                        k++;
                    };
                }
            }*/
            //вроде нормально, НО надо выбрать как рожать потомков
        }

        public void mutation(int MutationType)
        {
            int[] child = Children[(int)Math.Floor(rand.NextDouble() * Children.Length)];
            switch (MutationType)
            {
                case 1:
                    for (int i = 0; i < nu; i++)
                        if (rand.NextDouble() <= 0.1)
                            for (int j = 0; j < L / 17; j++)
                                Mutants[i] = mutOneDot(child, L);
                        else Mutants[i] = Children[(int)Math.Floor(rand.NextDouble() * Children.Length)];
                    break;
                case 2:
                    for (int i = 0; i < nu; i++)
                        if (rand.NextDouble() <= 0.1)
                            for (int j = 0; j < L / 17; j++)
                                Mutants[i] = mutTwoDot(child, L);
                        else Mutants[i] = Children[(int)Math.Floor(rand.NextDouble() * Children.Length)];
                    break;
                default:
                    return;
            }
            /*
            for (int i = 0; i < nu; i++)
            {
                if (rand.NextDouble() <= 0.1)
                {
                    switch (MutationType)
                    {
                        case 1: Mutants[i] = mutOneDot(child, L);
                            break;

                        case 2: Mutants[i] = mutTwoDot(child, L);
                            break;

                        default: continue;
                    }
                }
            }
             */
        }

        public void selection(int SelectionType)
        {

            int[][] R = summArr(Mutants, Children);//можно обойтись без этого массива и сохранить память
            //возможны проблемы с созданием массива
            R = summArr(R, Parents);

            switch (SelectionType)
            {
                case 1: R = lineRangSel(R, nu);
                    break;
                case 2: R = betaTournSel(R, nu);
                    break;

            }

            Parents = R;
        }

        public String takeAnswer()
        {
            Parents = notVozr(Parents);
            if (min == 0) min = Mu(AdaptationMatrix, Parents[0]);
            foreach (int[] x in Parents)
            {
                if (min > Mu(AdaptationMatrix, x)) min = Mu(AdaptationMatrix, x);
                if (max < Mu(AdaptationMatrix, x)) max = Mu(AdaptationMatrix, x);
            }
            String answer = "";
            foreach (int y in Parents[0])//Parents.Length - 1])
            {
                answer += " " + y;
            }
            answer += " | Mu " + Mu(AdaptationMatrix, Parents[0]) + ".\n";
            sum = Mu(AdaptationMatrix, Parents[0]);
            return answer;
        }
    }

}
