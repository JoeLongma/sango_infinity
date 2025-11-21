using Sango.Game.Player;
using System.Collections.Generic;
using System.Text;

namespace Sango.Game
{
    public enum CitySortTileType : int
    {
        Name = 0,

    }

    public enum CitySortGroupType : int
    {
        //自定义,功能独有
        Custom = 0,
        //状态
        State,
        //战力
        FightPower,
        //兵装
        Item,
        //资金
        Gold,
        //兵粮
        Food,
        //灾害
        Disaster,

        Max
    }

    public class CitySortFunction : Singleton<CitySortFunction>
    {
        public delegate string CityValueStrGet(City city);
        public delegate int CityValueGet(City city);
        public delegate int CitySortFunc(City city1, City city2);

        public City CurCity;

        public class SortTitle : ObjectSortTitle
        {
            public CityValueStrGet valueGetCall;
            public CitySortFunc citySortFunc;

            public override string GetValueStr(SangoObject obj)
            {
                return valueGetCall.Invoke((City)obj);
            }

            public override int Sort(SangoObject a, SangoObject b)
            {
                return citySortFunc.Invoke((City)a, (City)b);
            }
        }

        public void GetSortTitleGroup(CitySortGroupType citySortTileGroupType, List<ObjectSortTitle> titleList)
        {
            switch (citySortTileGroupType)
            {
                case CitySortGroupType.State:
                    {
                        titleList.Add(SortByName);
                        break;
                    }
                case CitySortGroupType.FightPower:
                    {
                        titleList.Add(SortByName);
                        titleList.Add(SortByTroopsLimit);
                        break;
                    }
                case CitySortGroupType.Item:
                    {
                        titleList.Add(SortByName);
                        break;
                    }
                case CitySortGroupType.Gold:
                    {
                        titleList.Add(SortByName);
                        break;
                    }
                case CitySortGroupType.Food:
                    {

                        titleList.Add(SortByName);
                        break;
                    }
                case CitySortGroupType.Disaster:
                    {

                        titleList.Add(SortByName);
                        break;
                    }
            }
        }

        public string GetSortTitleGroupName(CitySortGroupType citySortTileGroupType)
        {
            switch (citySortTileGroupType)
            {
                case CitySortGroupType.State: return "状态";
                case CitySortGroupType.FightPower: return "战力";
                case CitySortGroupType.Item: return "兵装";
                case CitySortGroupType.Gold: return "资金";
                case CitySortGroupType.Food: return "兵粮";
                case CitySortGroupType.Disaster: return "灾害";
            }

            return "";
        }


        public static SortTitle SortByName = new SortTitle()
        {
            name = "城池",
            width = 50,
            valueGetCall = x => x.Name,
            citySortFunc = (a, b) => a.Name.CompareTo(b.Name),
        };

        public static SortTitle SortByPersonCount = new SortTitle()
        {
            name = "现役",
            width = 50,
            valueGetCall = x => x.allPersons.Count.ToString(),
            citySortFunc = (a, b) => a.allPersons.Count.CompareTo(b.allPersons.Count),
        };

        public static SortTitle SortByTroops = new SortTitle()
        {
            name = "士兵",
            width = 50,
            valueGetCall = x => x.troops.ToString(),
            citySortFunc = (a, b) => a.troops.CompareTo(b.troops),
        };

        public static SortTitle SortByTroopsLimit = new SortTitle()
        {
            name = "士兵上限",
            width = 50,
            valueGetCall = x => x.TroopsLimit.ToString(),
            citySortFunc = (a, b) => a.TroopsLimit.CompareTo(b.TroopsLimit),
        };

        public static SortTitle SortByGold = new SortTitle()
        {
            name = "资金",
            width = 50,
            valueGetCall = x => x.gold.ToString(),
            citySortFunc = (a, b) => a.gold.CompareTo(b.gold),
        };

        public static SortTitle SortByGoldLimit = new SortTitle()
        {
            name = "资金上限",
            width = 50,
            valueGetCall = x => x.GoldLimit.ToString(),
            citySortFunc = (a, b) => a.GoldLimit.CompareTo(b.GoldLimit),
        };

        public static SortTitle SortByFood = new SortTitle()
        {
            name = "兵粮",
            width = 50,
            valueGetCall = x => x.food.ToString(),
            citySortFunc = (a, b) => a.food.CompareTo(b.food),
        };

        public static SortTitle SortByFoodLimit = new SortTitle()
        {
            name = "兵粮上限",
            width = 50,
            valueGetCall = x => x.FoodLimit.ToString(),
            citySortFunc = (a, b) => a.FoodLimit.CompareTo(b.FoodLimit),
        };

        public static SortTitle SortByLevel = new SortTitle()
        {
            name = "等级",
            width = 50,
            valueGetCall = x => x.CityLevelType.Name,
            citySortFunc = (a, b) => a.CityLevelType.Id.CompareTo(b.CityLevelType.Id),
        };

        public static SortTitle SortByIsFree = new SortTitle()
        {
            name = "空闲",
            width = 50,
            valueGetCall = x => x.freePersons.Count.ToString(),
            citySortFunc = (a, b) => a.freePersons.Count.CompareTo(b.freePersons.Count),
        };

        public static SortTitle SortByIsWild = new SortTitle()
        {
            name = "在野",
            width = 50,
            valueGetCall = x => x.wildPersons.Count.ToString(),
            citySortFunc = (a, b) => a.wildPersons.Count.CompareTo(b.wildPersons.Count),
        };

        public static SortTitle SortByBelongForce = new SortTitle()
        {
            name = "势力",
            width = 50,
            valueGetCall = x => x.BelongForce?.Name ?? "无",
            citySortFunc = (a, b) => SangoObject.Compare(a.BelongForce, b.BelongForce),
        };

        public static SortTitle SortByBelongCorps = new SortTitle()
        {
            name = "军团",
            width = 50,
            valueGetCall = x => x.BelongCorps?.Name ?? "无",
            citySortFunc = (a, b) => SangoObject.Compare(a.BelongCorps, b.BelongCorps),
        };

        public static SortTitle SortByBelongCity = new SortTitle()
        {
            name = "所属",
            width = 50,
            valueGetCall = x => x.BelongCity?.Name ?? "无",
            citySortFunc = (a, b) => SangoObject.Compare(a.BelongCity, b.BelongCity),
        };

    }

}
