using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Metaheuristic;//LAB Library

namespace ACOxTSP
{
    class Program
    {
        static void Main(string[] args)
        {
            int RunLength=2/*執行次數*/;
            
            TSP aco = new TSP();

            System.IO.StreamWriter sw = new System.IO.StreamWriter("OutPut.csv");//寫入檔案
          
            sw.WriteLine("Case" + "," + "Best"/*BestSolution*/+ "," + "AVG" + "," + "SD" + "," + "AVG Time");//寫入抬頭

            for (int problem = 0; problem < aco.problem.Length; problem++)
            {
                double[] BestFitness = new double[RunLength];/*存放每次執行的最佳解*/
                double BFSum = 0.0,BestOfEachRun = double.MaxValue;/*最小化問題*/

                System.Diagnostics.Stopwatch swTimer = new System.Diagnostics.Stopwatch();//計時器(單位:毫秒)

                aco.InitialProblem(problem);

                Console.WriteLine("Problem "+(problem+1) +" "+aco.problem[problem]);

                for (int run = 0; run < RunLength; run++)
                {
                    swTimer.Reset();/*計時器歸零*/
                    swTimer.Start();/*計時器開始*/
                   
                    aco.Init(40,aco.NumOfCity,0,aco.NumOfCity - 1,ACOOption.RepeatableOption.Nonrepeatable,ACOOption.CycleOption.Cycle);
                    aco.Run(4000,1,5,0.8,10);
                    //aco.Run(4000, 1, 5, 0.9, 0, 0.8, 1);  //local pheromone and global pheromone

                    BestFitness[run] = aco.GbestFitness;
                    BFSum += BestFitness[run];

                    if (BestFitness[run] < BestOfEachRun)
                    {/*找出每次RUN的最佳解中最好的那個*/
                        BestOfEachRun = BestFitness[run];
                    }

                    Console.WriteLine("No." + (run+1) +" Run : "+ "Best Finess = " + BestFitness[run]);

                }
                swTimer.Stop();/*計時器暫停*/
                double TimeCosts = swTimer.Elapsed.TotalMilliseconds / 1000.0;/*毫秒轉秒*/

                Console.WriteLine(aco.problem[problem] + "," + BestOfEachRun + "," + (BFSum / RunLength) + "," + SD(BestFitness) + "," + TimeCosts / RunLength);//印出
                sw.WriteLine(aco.problem[problem] + "," + BestOfEachRun + "," + (BFSum / RunLength) + "," + SD(BestFitness) + "," + TimeCosts / RunLength);//儲存
            }
            sw.Close();
            Console.Read();
        }
        public static double SD(double[] fit)
        {
            double sum = 0.0, average;
            for (int i = 0; i < fit.Length; i++)
                sum += fit[i];
            average = sum / fit.Length;
            sum = 0.0;
            for (int i = 0; i < fit.Length; i++)
                sum += (Math.Pow(fit[i] - average, 2));
            return Math.Pow(sum / fit.Length, 0.5);
        }
    }
}
