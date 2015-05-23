using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GenAlg
{
    class Program
    {

        static int[][] Parsing(String path, StreamWriter sw)
        {
            int[][] arr;
            int dimension = 0;
            int i = 0, j = 0;
            using (StreamReader sReader = File.OpenText(path))
            {
                for (int k = 0; !sReader.EndOfStream && k < 7; k++)
                {
                    string lineOfText = sReader.ReadLine();
                    if (lineOfText != null)
                    {
                        string[] split = lineOfText.Split(new Char[] { ' ', '\t' },
                            StringSplitOptions.RemoveEmptyEntries);

                        foreach (string x in split)
                        {
                            if (x != "EDGE_WEIGHT_SECTION")
                            {
                                Console.Write(x + ' ');
                                sw.Write(x + ' ');
                            }
                        }
                        Console.WriteLine();
                        sw.WriteLine();

                        if (split[0] == "DIMENSION:")
                        {
                            dimension = Convert.ToInt32(split[1]);
                        }
                    }
                }

                arr = new int[dimension][];
                arr[0] = new int[dimension];

                while (!sReader.EndOfStream)
                {
                    string lineOfText = sReader.ReadLine();
                    if (lineOfText != "EOF")
                    {
                        string[] split = lineOfText.Split(new Char[] { ' ', '\t' },
                            StringSplitOptions.RemoveEmptyEntries);

                        foreach (string x in split)
                        {
                            arr[i][j++] = Convert.ToInt32(x);
                            if (j >= dimension && i + 1 < dimension)
                            {
                                j = 0;
                                i++;
                                arr[i] = new int[dimension];
                            }
                        }
                    }
                }
            }
            return arr;
        }


        static void Run(int times, StreamWriter sw, int g, int p, int[][] arr, int cross, int sel, int mut1, int mut2 = 0)
        {
            gaTSP alg;
            string message = "CROSSOVER: " + cross + "\n MUTATIONS: " + mut1 + " " + mut2 + "\n SELECTION: " + sel;
            int sum = 0;
            int min = 0;
            int max = 0;
            Console.WriteLine(message + "\nITERATIONS: ");
            sw.WriteLine("CROSSOVER: " + cross);
            sw.WriteLine("MUTATIONS: " + mut1 + " " + mut2);
            sw.WriteLine("SELECTION: " + sel);
            sw.WriteLine("GENERATIONS: " + g);
            sw.WriteLine("POPULATION: " + p);
            sw.WriteLine();
            for (int i = 0; i < times; i++)
            {
                alg = new gaTSP(arr, p);
                for (int j = 0; j < g; j++)
                {
                    alg.crossover(cross);
                    alg.mutation(mut1);
                    alg.mutation(mut2);
                    alg.selection(sel);
                }
                sw.WriteLine(alg.takeAnswer());
                Console.Write((i + 1) + " ");
                sum += alg.sum;
                min = gaTSP.min;
                max = gaTSP.max;
                if (i == (times - 1))
                {
                    gaTSP.min = 0;
                    gaTSP.max = 0;
                }
            }
            sw.WriteLine("MINIMUM: " + min);
            sw.WriteLine("AVERAGE: " + sum / times);
            sw.WriteLine("MAXIMUM: " + max);
            Console.WriteLine("min= " + min + " sre= " + sum / times + " max= " + max + "\n");
            Console.WriteLine();
            sw.WriteLine();

        }

        static void Main(string[] args)
        {
            problem_data pd = new problem_data();
            int[][] arr;
            int retries = 50;
            int gen = 500;
            int pop = 70;

            string str = "c:/Users/Admin/Documents/GA experiments/" + DateTime.Now.ToString("dd.MM.yy HH-mm-ss") + ".txt";
            using (StreamWriter sw = new StreamWriter(str))
            {
                FileInfo[] cFiles = new DirectoryInfo(@"C:\ATSP\").GetFiles();

                FileInfo x = cFiles[1];
                //foreach (FileInfo x in cFiles)
                {
                    arr = Parsing(x.FullName, sw);
                    sw.WriteLine("Generations: " + gen + "\nPopulation: " + pop);
                    //Run(retries, sw, gen, pop, arr, 1, 1, 1);
                    //Run(retries, sw, gen, pop, arr, 1, 1, 2);
                    //Run(retries, sw, gen, pop, arr, 1, 1, 1, 2);
                    //Run(retries, sw, gen, pop, arr, 1, 2, 1);
                    //Run(retries, sw, gen, pop, arr, 1, 2, 2);
                    Run(retries, sw, gen, pop, arr, 1, 2, 1, 2);
                }

                /*
                gen = 1000;
                pop = 70;
                sw.WriteLine("\nGenerations: " + gen + "\nPopulation: " + pop);
                Run("ftv33", retries, sw, gen, pop, pd.ftv33, 1, 1, 1);
                Run("ftv33", retries, sw, gen, pop, pd.ftv33, 1, 1, 2);
                Run("ftv33", retries, sw, gen, pop, pd.ftv33, 1, 1, 1, 2);
                Run("ftv33", retries, sw, gen, pop, pd.ftv33, 1, 2, 1);
                Run("ftv33", retries, sw, gen, pop, pd.ftv33, 1, 2, 2);
                Run("ftv33", retries, sw, gen, pop, pd.ftv33, 1, 2, 1, 2);

                gen = 1400;
                pop = 70;
                sw.WriteLine("\nGenerations: " + gen + "\nPopulation: " + pop);
                Run("ft53", retries, sw, gen, pop, pd.ft53, 1, 1, 1);
                Run("ft53", retries, sw, gen, pop, pd.ft53, 1, 1, 2);
                Run("ft53", retries, sw, gen, pop, pd.ft53, 1, 1, 1, 2);
                Run("ft53", retries, sw, gen, pop, pd.ft53, 1, 2, 1);
                Run("ft53", retries, sw, gen, pop, pd.ft53, 1, 2, 2);
                Run("ft53", retries, sw, gen, pop, pd.ft53, 1, 2, 1, 2);

                gen = 3300;
                pop = 100;
                sw.WriteLine("\nGenerations: " + gen + "\nPopulation: " + pop);
                Run("ftv70", retries, sw, gen, pop, pd.ftv70, 1, 1, 1);
                Run("ftv70", retries, sw, gen, pop, pd.ftv70, 1, 1, 2);
                Run("ftv70", retries, sw, gen, pop, pd.ftv70, 1, 1, 1, 2);
                Run("ftv70", retries, sw, gen, pop, pd.ftv70, 1, 2, 1);
                Run("ftv70", retries, sw, gen, pop, pd.ftv70, 1, 2, 2);
                Run("ftv70", retries, sw, gen, pop, pd.ftv70, 1, 2, 1, 2);

                gen = 7000;
                pop = 200;
                sw.WriteLine("\nGenerations: " + gen + "\nPopulation: " + pop);
                Run("ftv170", retries, sw, gen, pop, pd.ftv170, 1, 1, 1);
                Run("ftv170", retries, sw, gen, pop, pd.ftv170, 1, 1, 2);
                Run("ftv170", retries, sw, gen, pop, pd.ftv170, 1, 1, 1, 2);
                Run("ftv170", retries, sw, gen, pop, pd.ftv170, 1, 2, 1);
                Run("ftv170", retries, sw, gen, pop, pd.ftv170, 1, 2, 2);
                Run("ftv170", retries, sw, gen, pop, pd.ftv170, 1, 2, 1, 2);
                */

                /*
                sw.WriteLineAsync(DateTime.Now.ToString());
                
                Console.WriteLine("\nftv33\n1 1");
                sw.WriteLine("\nftv33\n1 1");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ftv33, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);

                        alg.mutation(1);
                        alg.selection(1);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nftv33\n2 1");
                sw.WriteLine("\nftv33\n2 1");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ftv33, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);

                        alg.mutation(2);
                        alg.selection(1);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nftv33\n1 2");
                sw.WriteLine("\nftv33\n1 2");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ftv33, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);

                        alg.mutation(1);
                        alg.selection(2);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nftv33\n2 2");
                sw.WriteLine("\nftv33\n2 2");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ftv33, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);

                        alg.mutation(2);
                        alg.selection(2);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nftv33\n12 1");
                sw.WriteLine("\nftv33\n12 1");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ftv33, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);
                        alg.mutation(1);
                        alg.mutation(2);
                        alg.selection(1);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + "\nsre= " + sum / KOL + "\nmax= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nftv33\n12 2");
                sw.WriteLine("\nftv33\n12 2");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ftv33, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);
                        alg.mutation(1);
                        alg.mutation(2);
                        alg.selection(2);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                ///

                J = 2200;
                Console.WriteLine("\nft53\n1 1");
                sw.WriteLine("\nft53\n1 1");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ft53, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);

                        alg.mutation(1);
                        alg.selection(1);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nft53\n2 1");
                sw.WriteLine("\nft53\n2 1");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ft53, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);

                        alg.mutation(2);
                        alg.selection(1);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nft53\n1 2");
                sw.WriteLine("\nftv33\n1 2");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ft53, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);

                        alg.mutation(1);
                        alg.selection(2);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nft53\n2 2");
                sw.WriteLine("\nft53\n2 2");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ft53, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);

                        alg.mutation(2);
                        alg.selection(2);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nft53\n12 1");
                sw.WriteLine("\nft53\n12 1");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ft53, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);
                        alg.mutation(1);
                        alg.mutation(2);
                        alg.selection(1);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nft53\n12 2");
                sw.WriteLine("\nft53\n12 2");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ft53, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);
                        alg.mutation(1);
                        alg.mutation(2);
                        alg.selection(2);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                ///

                J = 3300;
                Console.WriteLine("\nftv70\n1 1");
                sw.WriteLine("\nftv70\n1 1");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ftv33, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);

                        alg.mutation(1);
                        alg.selection(1);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nftv70\n2 1");
                sw.WriteLine("\nftv70\n2 1");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ftv33, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);

                        alg.mutation(2);
                        alg.selection(1);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nftv70\n1 2");
                sw.WriteLine("\nftv70\n1 2");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ftv33, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);

                        alg.mutation(1);
                        alg.selection(2);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nftv70\n2 2");
                sw.WriteLine("\nftv70\n2 2");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ftv33, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);

                        alg.mutation(2);
                        alg.selection(2);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nftv70\n12 1");
                sw.WriteLine("\nftv70\n12 1");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ftv33, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);
                        alg.mutation(1);
                        alg.mutation(2);
                        alg.selection(1);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                Console.WriteLine("\nftv70\n12 2");
                sw.WriteLine("\nftv70\n12 2");
                for (int j = 0; j < KOL; j++)
                {
                    alg = new gaTSP(ftv33, 70);
                    for (int i = 0; i < J; i++)
                    {
                        alg.crossover(1);
                        alg.mutation(1);
                        alg.mutation(2);
                        alg.selection(2);
                    }
                    alg.takeAnswer();
                    Console.Write(j + " ");
                    sum += alg.sum;
                    min = gaTSP.min;
                    max = gaTSP.max; if (j == (KOL - 1)) { gaTSP.max = 0; gaTSP.min = 0; }
                }
                sw.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); Console.WriteLine("\nmin= " + min + " sre= " + sum / KOL + " max= " + max); sum = 0;

                */
            }
        }



    }
}