using System;

namespace Sango.Game
{
    public class GameUtility
    {
        public static System.Random RandomDigit = new System.Random();
        public static int Random(int maxValue)
        {
            if (maxValue <= 0)
            {
                return 0;
            }
            return RandomDigit.Next(maxValue);
        }
        public static double Random()
        {
            return RandomDigit.NextDouble();
        }
        public static int GetRandomValue(int a, int b)
        {
            int num;
            int num2;
            if (b == 0)
            {
                return 0;
            }
            if (b > 0)
            {
                num = a / b;
                num2 = a % b;
                if ((num2 > 0) && (Random(b) < num2))
                {
                    num++;
                }
                return num;
            }
            b = Math.Abs(b);
            num = a / b;
            num2 = a % b;
            if ((num2 > 0) && (Random(b) < num2))
            {
                num++;
            }
            return -num;
        }
        public static int GetBigRandomValue(int a, int b)
        {
            int num;
            int num2;
            if (b == 0)
            {
                return 0;
            }
            if (b > 0)
            {
                num = (int)(a / b);
                num2 = (int)(a % b);
                if ((num2 > 0) && (Random((int)b) < num2))
                {
                    num++;
                }
                return num;
            }
            //b = Math.Abs(b);
            b = -b;
            num = (int)(a / b);
            num2 = (int)(a % b);
            if ((num2 > 0) && (Random((int)b) < num2))
            {
                num++;
            }
            return -num;
        }
        public static bool Chance(int chance)
        {
            if (chance <= 0)
            {
                return false;
            }
            return ((chance >= 100) || (Random(100) < chance));
        }
        public static bool Chance(int chance, int root)
        {
            if (chance <= 0)
            {
                return false;
            }
            return ((chance >= root) || (Random(root) < chance));
        }
        public static int Random(int min, int max)
        {
            return Random(Math.Abs(max - min) + 1) + Math.Min(max, min);
        }
        public static int RandomGaussian(double mean, double var)
        {
            double u1 = Random();
            double u2 = Random();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2);
            return (int)Math.Round(mean + (var / 3) * randStdNormal);
        }
        public static int RandomGaussianRange(int lo, int hi)
        {
            return RandomGaussian((hi + lo) / 2.0, Math.Abs(hi - lo) / 2.0);
        }

        public static int Square(int num)
        {
            return (num * num);
        }

        public static int Method_PersonBuildAbility(int v)
        {
            return 2 * v;
        }

        public static int Method_SecurityAbility(int v)
        {
            return Math.Max(1, (int)(Math.Pow(v, 0.5)) - 5);
        }

        public static int Method_FarmingAbility(int v)
        {
            return Math.Max(1, (int)(Math.Pow(v, 0.5)) - 5);
        }

        public static int Method_DevelopAbility(int v)
        {
            return Math.Max(1, (int)(Math.Pow(v, 0.5)) - 5);
        }

        // 这里传入的v是放大了10倍的
        public static int Method_RecuritTroop(int v, int buildingNum)
        {
            int percent = 1000;
            for (int i = 1; i < buildingNum; ++i)
                percent += (int)(Math.Pow(100, 0.5f / buildingNum) * 100f);

            return ((int)(v * 8.567f / 10f - 1) + 1500) * percent / 1000;
        }

        public static int Method_TrainTroop(int v)
        {
            return Math.Max(1, (int)(Math.Pow(v, 0.5)) * 2 - 10);
        }

        // 这里传入的v是放大了10倍的
        public static int Method_CreateItems(int v, int buildingNum)
        {
            int percent = 1000;
            for (int i = 1; i < buildingNum; ++i)
                percent += (int)(Math.Pow(100, 0.5f / buildingNum) * 100f);
            return (v * 15 / 10 + 1500) * percent / 1000;
        }

        public static int Method_TroopBuildAbility(Troop troop)
        {
            return (int)(troop.BuildPower * Math.Min(1f, (float)troop.troops / 1000) + 300 * (1f - Math.Pow(1.0 - (Math.Max(troop.troops, 5000.0) / 5000.0), 1.451)));
        }
    }
}
