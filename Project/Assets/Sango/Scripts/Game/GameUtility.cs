using Sango.Game.Tools;
using System;
using System.Collections.Generic;

namespace Sango.Game
{
    public class GameUtility
    {

        public static OverrideData<int> IntOverrideData = new OverrideData<int>(0);
        public static OverrideData<float> FloatOverrideData = new OverrideData<float>(0);
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

        public static int Method_PersonBuildAbility(Person p1, Person p2, Person p3)
        {
            int v1 = p1?.BaseBuildAbility ?? 0;
            int v2 = p2?.BaseBuildAbility ?? 0;
            int v3 = p3?.BaseBuildAbility ?? 0;

            int total = v1 + v2 + v3;
            return total * 2;
        }

        public static int Method_PersonBuildAbility(List<Person> people)
        {
            int maxValue = 0;
            for (int i = 0; i < people.Count; ++i)
            {
                Person person = people[i];
                if (person == null) continue;
                maxValue += person.BaseBuildAbility;
            }
            return maxValue * 2;
        }
        public static int Method_PersonBuildAbility(Person[] people)
        {
            int maxValue = 0;
            for (int i = 0; i < people.Length; ++i)
            {
                Person person = people[i];
                if (person == null) continue;
                maxValue += person.BaseBuildAbility;
            }
            return maxValue * 2;
        }
        public static int Method_PersonBuildAbility(SangoObjectList<Person> people)
        {
            int maxValue = 0;
            for (int i = 0; i < people.Count; ++i)
            {
                Person person = people[i];
                if (person == null) continue;
                maxValue += person.BaseBuildAbility;
            }
            return maxValue * 2;
        }

        public static int Method_SecurityAbility(int v, int buildingTotalLevel)
        {
            int percent = 0;
            for (int i = 1; i <= buildingTotalLevel; ++i)
                percent += (int)(Math.Pow(v, 0.5f / i));
            return Math.Max(1, percent - 10);
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
        public static int Method_RecuritTroops(int v, int buildingTotalLevel)
        {
            int percent = 0;
            for (int i = 1; i <= buildingTotalLevel; ++i)
                percent += (int)(Math.Pow(100, 0.5f / i) * 60);

            return ((int)(v * 8.567f / 10f - 1) + 1500) * percent / 1000;
        }

        /// 训练值计算公式
        public static int Method_TrainTroops(int v, int buildingTotalLevel)
        {
            int percent = 0;
            for (int i = 1; i <= buildingTotalLevel; ++i)
                percent += (int)(Math.Pow(v, 0.5f / i));
            return Math.Max(1, percent - 3);
        }

        // 交易比列计算公式
        public static int Method_Trade(int v)
        {
            int percent = 100;
            percent += Math.Max(0, v - 70) / 2;
            return percent;
        }

        // 这里传入的v是放大了10倍的
        public static int Method_CreateItems(int v, int buildingTotalLevel)
        {
            /*
             * 
             * 枪、戟、弩、马一次的生产数量（相关属性：智力，特技：能吏）
                （以Lv.1设施为准，LV.2设施为1.2倍，Lv.3设施为1.5倍）
                计算基准值q＝1000＋10xa＋5(xb＋xc)
                最终值Q＝β[T×α(q)]
                其中：有特技“能吏”则T为2，否则为1
                例：智力为84、83、83的三个武将，在Lv.3的设施中，一回合生产枪的数量为：
                1.5×[1000＋10×84＋5×(83＋83)]＝4005
             * 
             */
            int percent = 0;
            for (int i = 1; i <= buildingTotalLevel; ++i)
                percent += (int)(Math.Pow(100, 0.5f / i) * 50);

            return (v + 1000) * 15 / 10 * percent / 1000;
        }

        /// <summary>
        /// 计算建造船只所需的旬数
        /// </summary>
        /// <param name="v"></param>
        /// <param name="buildingTotalLevel"></param>
        /// <returns></returns>
        public static int Method_CreateBoatCounter(int maxValue, int subValue, int buildingTotalLevel)
        {
            /*
              舰船生产所耗回合数（相关属性：智力，特技：造船，设施：练兵所）
              计算基准值ds＝(550－2.5xa)÷30－(xb+xc)÷24
              最终值Ds＝β{[α(ds)－S]÷T}
              其中：有特技“发明”则T为2，否则为1；S为“造船厂等级/3”，最大为3
              ※舰船生产所需的回合数最大为9
              例：智力为88、72、72的三个武将，在有练兵所的情况下，生产楼船所耗旬数为：(550－2.5×88)÷30－(72＋72)÷24－2＝5－2＝3
              如果上述武将中有特技“造船”，则所耗旬数为β(3÷2)＝1
          */
            return Math.Max(1, Math.Min(9, (550 - maxValue * 10 / 4) / 30 - (subValue) / 24 - Math.Min(buildingTotalLevel / 3, 3)));
        }


        /// <summary>
        /// 计算建造器械所需的旬数
        /// </summary>
        /// <param name="v"></param>
        /// <param name="buildingTotalLevel"></param>
        /// <returns></returns>
        public static int Method_CreateMachineCounter(int maxValue, int subValue, int buildingTotalLevel)
        {
            /*
            兵器生产所耗回合数（相关属性：智力，特技：发明）
            计算基准值dv＝(550－2.5xa)÷30－(xb+xc)÷24
            最终值Dv＝β[α(dv)÷T]
            其中：有特技“发明”则T为2，否则为1
            ※兵器生产所需的回合数最大为9
            设施“练兵所”生产楼船、斗舰的时长再除以2并向上取整
            例：智力为98、95、77的三个武将，生产冲车所耗旬数为：
            (550－2.5×98)÷30－(95+77)÷24＝3
            如果上述武将中有特技为“发明”，则所耗旬数为β(3÷2)＝1
            智力为88、84、84的三个武将，生产投石所耗旬数为：
            (550－2.5×88)÷30－(84＋84)÷24＝4
          */
            return Math.Max(1, Math.Min(9, (550 - maxValue * 10 / 4) / 30 - (subValue) / 24 - Math.Min(buildingTotalLevel / 3, 3)));
        }

        public static int Method_TroopBuildAbility(Troop troop)
        {
            return (int)(troop.BuildPower * Math.Min(1f, (float)troop.troops / 1000) + 300 * (1f - Math.Pow(1.0 - (Math.Max(troop.troops, 5000.0) / 5000.0), 1.451)));
        }
    }
}
