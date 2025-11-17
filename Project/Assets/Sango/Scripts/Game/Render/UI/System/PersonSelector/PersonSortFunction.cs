using System.Collections.Generic;
using System.Text;

namespace Sango.Game
{
    public enum PersonSortTileType : int
    {
        Name = 0,

    }

    public enum PersonSortGroupType : int
    {
        //自定义,功能独有
        Custom = 0,
        //所属
        Belong,
        //能力
        Attribute,
        //特技
        Feature,
        //适应
        Ability,
        //任务
        Mission,
        //个人
        Personal,
        //血缘
        Consanguinity,

        Max
    }

    public class PersonSortFunction : Singletion<PersonSortFunction>
    {
        public delegate string PersonValueStrGet(Person person);
        public delegate int PersonValueGet(Person person);
        public delegate int PersonSortFunc(Person person1, Person person2);

        public class SortTitle
        {
            public string name;
            public int width;
            public PersonValueStrGet valueGetCall;
            public PersonSortFunc personSortFunc;
        }

        public void GetSortTitleGroup(PersonSortGroupType personSortTileGroupType, List<SortTitle> titleList)
        {
            switch (personSortTileGroupType)
            {
                case PersonSortGroupType.Belong:
                    {
                        titleList.Add(SortByName);
                        break;
                    }
                case PersonSortGroupType.Attribute:
                    {
                        titleList.Add(SortByName);
                        titleList.Add(SortByTroopsLimit);
                        titleList.Add(SortByCommand);
                        titleList.Add(SortByStrength);
                        titleList.Add(SortByIntelligence);
                        titleList.Add(SortByPolitics);
                        titleList.Add(SortByGlamour);
                        titleList.Add(SortByFeatureList);
                        break;
                    }
                case PersonSortGroupType.Feature:
                    {
                        titleList.Add(SortByName);
                        titleList.Add(SortByFeatureList);
                        break;
                    }
                case PersonSortGroupType.Ability:
                    {
                        titleList.Add(SortByName);
                        titleList.Add(SortByHalberdLv);
                        titleList.Add(SortByCrossbowLv);
                        titleList.Add(SortBySpearLv);
                        titleList.Add(SortByHorseLv);
                        titleList.Add(SortByWaterLv);
                        titleList.Add(SortByMachineLv);
                        titleList.Add(SortByFeatureList);
                        break;
                    }
                case PersonSortGroupType.Mission:
                    {

                        titleList.Add(SortByName);
                        break;
                    }
                case PersonSortGroupType.Personal:
                    {

                        titleList.Add(SortByName);
                        break;
                    }
                case PersonSortGroupType.Consanguinity:
                    {

                        titleList.Add(SortByName);
                        break;
                    }
            }
        }

        public string GetSortTitleGroupName(PersonSortGroupType personSortTileGroupType)
        {
            switch (personSortTileGroupType)
            {
                case PersonSortGroupType.Belong:        return "所属";
                case PersonSortGroupType.Attribute:     return "能力";
                case PersonSortGroupType.Feature:       return "特技";
                case PersonSortGroupType.Ability:       return "适应";
                case PersonSortGroupType.Mission:       return "任务";
                case PersonSortGroupType.Personal:      return "个人";
                case PersonSortGroupType.Consanguinity: return "血缘";
            }

            return "";
        }


        public static SortTitle SortByName = new SortTitle()
        {
            name = "武将",
            width = 50,
            valueGetCall = x => x.Name,
            personSortFunc = (a, b) => a.Name.CompareTo(b.Name),
        };

        public static SortTitle SortByTroopsLimit = new SortTitle()
        {
            name = "指挥",
            width = 50,
            valueGetCall = x => x.TroopsLimit.ToString(),
            personSortFunc = (a, b) => a.TroopsLimit.CompareTo(b.TroopsLimit),
        };

        public static SortTitle SortByCommand = new SortTitle()
        {
            name = "统率",
            width = 50,
            valueGetCall = x => x.Command.ToString(),
            personSortFunc = (a, b) => a.Command.CompareTo(b.Command),
        };

        public static SortTitle SortByStrength = new SortTitle()
        {
            name = "武力",
            width = 50,
            valueGetCall = x => x.Strength.ToString(),
            personSortFunc = (a, b) => a.Strength.CompareTo(b.Strength),
        };

        public static SortTitle SortByIntelligence = new SortTitle()
        {
            name = "智力",
            width = 50,
            valueGetCall = x => x.Intelligence.ToString(),
            personSortFunc = (a, b) => a.Intelligence.CompareTo(b.Intelligence),
        };

        public static SortTitle SortByPolitics = new SortTitle()
        {
            name = "政治",
            width = 50,
            valueGetCall = x => x.Politics.ToString(),
            personSortFunc = (a, b) => a.Politics.CompareTo(b.Politics),
        };

        public static SortTitle SortByGlamour = new SortTitle()
        {
            name = "魅力",
            width = 50,
            valueGetCall = x => x.Glamour.ToString(),
            personSortFunc = (a, b) => a.Glamour.CompareTo(b.Glamour),
        };

        public static SortTitle SortBySpearLv = new SortTitle()
        {
            name = "枪兵",
            width = 50,
            valueGetCall = x => Scenario.Cur.Variables.GetAbilityName(x.SpearLv),
            personSortFunc = (a, b) => a.SpearLv.CompareTo(b.SpearLv),
        };

        public static SortTitle SortByHalberdLv = new SortTitle()
        {
            name = "盾兵",
            width = 50,
            valueGetCall = x => Scenario.Cur.Variables.GetAbilityName(x.HalberdLv),
            personSortFunc = (a, b) => a.HalberdLv.CompareTo(b.HalberdLv),
        };

        public static SortTitle SortByCrossbowLv = new SortTitle()
        {
            name = "弓兵",
            width = 50,
            valueGetCall = x => Scenario.Cur.Variables.GetAbilityName(x.CrossbowLv),
            personSortFunc = (a, b) => a.CrossbowLv.CompareTo(b.CrossbowLv),
        };

        public static SortTitle SortByHorseLv = new SortTitle()
        {
            name = "骑兵",
            width = 50,
            valueGetCall = x => Scenario.Cur.Variables.GetAbilityName(x.HorseLv),
            personSortFunc = (a, b) => a.HorseLv.CompareTo(b.HorseLv),
        };

        public static SortTitle SortByWaterLv = new SortTitle()
        {
            name = "水军",
            width = 50,
            valueGetCall = x => Scenario.Cur.Variables.GetAbilityName(x.WaterLv),
            personSortFunc = (a, b) => a.WaterLv.CompareTo(b.WaterLv),
        };

        public static SortTitle SortByMachineLv = new SortTitle()
        {
            name = "兵器",
            width = 50,
            valueGetCall = x => Scenario.Cur.Variables.GetAbilityName(x.MachineLv),
            personSortFunc = (a, b) => a.MachineLv.CompareTo(b.MachineLv),
        };

        public static SortTitle SortByFeatureList = new SortTitle()
        {
            name = "特技",
            width = 50,
            valueGetCall = x =>
            {

                StringBuilder sb = new StringBuilder();
                if (x.FeatureList != null)
                {
                    for (int i = 0; i < x.FeatureList.Count; i++)
                    {
                        sb.Append(x.FeatureList[i].Name);
                        if (i < x.FeatureList.Count - 1)
                            sb.Append(", ");
                    }
                }
                return sb.ToString();
            },
            personSortFunc = (a, b) =>
            {
                if (a.FeatureList == null && b.FeatureList == null)
                    return 0;
                if (a.FeatureList != null && b.FeatureList == null)
                    return -1;
                if (a.FeatureList == null && b.FeatureList != null)
                    return 1;
                return a.FeatureList.Count.CompareTo(b.FeatureList.Count);
            }
        };

    }

}
