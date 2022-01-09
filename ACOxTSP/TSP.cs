using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Metaheuristic;

namespace ACOxTSP
{
    class TSP : ACO
    {
        public string[] problem = new string[] { "Bays29", "Berlin52", "Eil51", "Eil76", "Pr76", "St70", "Oliver30" };

        public double[][] Distance;
        public int NumOfCity;

        #region 載入TSP

        public void InitialProblem(int problemIndex)
        {/*根據TSP問題的資料不同，有不同的初始化方式*/
            /*
             * 目前實驗的問題都是完整且對稱的TSP問題
             * 完整就是每個城市都能到達除了自己以外的所有城鎮。
             * 對稱就是N城鎮到M城鎮的距離 = M城鎮到N城鎮的距離
             */

            string FileLoad = "../../TSP/" + problem[problemIndex]+".txt";

            #region 抓取城市數量
            string CatchNumOfCity = "";
            foreach (char c in problem[problemIndex])
            {/*抓字串中的數字部分*/
                if ((int)c >= 48 && (int)c <= 57)
                    CatchNumOfCity += c;
            }
            NumOfCity = int.Parse(CatchNumOfCity);
            #endregion

            #region 距離矩陣資料
            if (problemIndex == 0)
            {/*已經算好距離矩陣的二維資料->直接讀取*/

                Distance = new double[NumOfCity][];
                for (int city = 0; city < NumOfCity; city++)
                    Distance[city] = new double[NumOfCity];
                readStreetDistance(FileLoad);
            }
            #endregion

            #region 座標位置資料
            else
            {/*僅給予各城市之座標，自行計算二維距離矩陣*/
                readGeographicalDistance(FileLoad);
            }
            #endregion
        }
        
        public void readGeographicalDistance(string file)
        {
            System.IO.StreamReader str = new System.IO.StreamReader(file);
            string all = str.ReadToEnd();
            string[] c = all.Split(new char[] { ' ', '\n' });
            Distance = new double[int.Parse(c[0])][];
            for (int i = 0; i < int.Parse(c[0]); i++)
                Distance[i] = new double[int.Parse(c[0])];
            for (int i = 0; i < int.Parse(c[0]); i++)
            {
                for (int j = 0; j < int.Parse(c[0]); j++)
                {
                    int t1 = 3 * i + 1, t2 = 3 * j + 1;
                    double x1 = Double.Parse(c[t1 + 1]), y1 = Double.Parse(c[t1 + 2]);
                    double x2 = Double.Parse(c[t2 + 1]), y2 = Double.Parse(c[t2 + 2]);
                    Distance[i][j] = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                }
            }
        }
        public void readStreetDistance(string file)
        {
            System.IO.StreamReader str = new System.IO.StreamReader(file);
            string all = str.ReadToEnd();
            string[] c = all.Split(new char[] { ' ', '\n' });
            int index = 0;
            for (int i = 0; i < NumOfCity; i++)
            {
                for (int j = 0; j < NumOfCity; j++)
                {
                    for (int k = index; k < c.Length; k++)
                    {
                        if (c[k] != "")
                        {
                            index = k + 1;
                            break;
                        }
                    }
                    Distance[i][j] = Double.Parse(c[index - 1]);
                }
            }
        }

        #endregion

        public override void InitVisibility()
        {//初始化Library之中的Visibility , Library初始化時，會自動呼叫此函式執行，所以不要執行這部分。
            for (int i = 0; i < Visibility.GetLength(0); i++)
                for (int j = 0; j < Visibility.GetLength(1); j++)
                    Visibility[i, j] = 1 / Distance[i][j];
        }

        public override double Fitness(int[] solution)
        {
            double sum = 0;
            for (int j = 0; j < solution.GetLength(0) - 1; j++)
                sum = sum + Distance[solution[j]][solution[j + 1]];
            sum = sum + Distance[solution[solution.GetLength(0) - 1]][solution[0]];

            return sum;
        }
    }
}
