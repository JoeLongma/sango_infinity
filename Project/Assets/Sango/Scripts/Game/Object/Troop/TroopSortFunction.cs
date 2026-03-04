namespace Sango.Game
{
    public class TroopSortFunction : Singleton<TroopSortFunction>
    {
        public delegate string TroopValueStrGet(Troop troop);
        public delegate T TroopValueGet<T>(Troop troop);
        public delegate int TroopSortFunc(Troop troop1, Troop troop2);

        public class SortTitle<T> : ObjectSortTitle
        {
            public TroopValueStrGet valueStrGetCall;
            public TroopSortFunc valueSortFunc;
            public TroopValueGet<T> valueGetCall;

            public  T GetValue(SangoObject obj)
            {
                return valueGetCall.Invoke((Troop)obj);
            }

            public override string GetValueStr(SangoObject obj)
            {
                return valueStrGetCall.Invoke((Troop)obj);
            }

            public override int Sort(SangoObject a, SangoObject b)
            {
                return valueSortFunc.Invoke((Troop)a, (Troop)b);
            }

            public SortTitle<T> Copy()
            {
                return new SortTitle<T>
                {
                    name = name,
                    alignment = alignment,
                    width = width,
                    valueStrGetCall = valueStrGetCall,
                    valueGetCall = valueGetCall,
                    valueSortFunc = valueSortFunc,
                };
            }
        }

        public static SortTitle<string> SortByName = new SortTitle<string>()
        {
            name = "武将",
            width = 80,
            valueGetCall = x => x.Name,
            valueStrGetCall = x => x.Name,
            valueSortFunc = (a, b) => a.Name.CompareTo(b.Name),
        };

        public static SortTitle<int> SortByDefence = new SortTitle<int>()
        {
            name = "防御",
            width = 50,
            valueStrGetCall = x => x.Defence.ToString(),
            valueGetCall = x => x.Defence,
            valueSortFunc = (a, b) => a.Defence.CompareTo(b.Defence),
        };
        public static SortTitle<int> SortByAttack = new SortTitle<int>()
        {
            name = "攻击",
            width = 50,
            valueStrGetCall = x => x.Attack.ToString(),
            valueGetCall = x => x.Attack,
            valueSortFunc = (a, b) => a.Attack.CompareTo(b.Attack),
        };
        public static SortTitle<int> SortByMoveability = new SortTitle<int>()
        {
            name = "移动",
            width = 50,
            valueStrGetCall = x => x.MoveAbility.ToString(),
            valueGetCall = x => x.MoveAbility,
            valueSortFunc = (a, b) => a.MoveAbility.CompareTo(b.MoveAbility),
        };
        public static SortTitle<int> SortByBuild = new SortTitle<int>()
        {
            name = "建设",
            width = 50,
            valueStrGetCall = x => x.BuildPower.ToString(),
            valueGetCall = x => x.BuildPower,
            valueSortFunc = (a, b) => a.BuildPower.CompareTo(b.BuildPower),
        };

        public static SortTitle<int> SortByIntelligence = new SortTitle<int>()
        {
            name = "智力",
            width = 50,
            valueStrGetCall = x => x.Intelligence.ToString(),
            valueGetCall = x => x.Intelligence,
            valueSortFunc = (a, b) => a.Intelligence.CompareTo(b.Intelligence),
        };

        public static SortTitle<int> SortByGold = new SortTitle<int>()
        {
            name = "资金",
            width = 50,
            valueStrGetCall = x => x.gold.ToString(),
            valueGetCall = x => x.gold,
            valueSortFunc = (a, b) => a.gold.CompareTo(b.gold),
        };

        public static SortTitle<int> SortByFood = new SortTitle<int>()
        {
            name = "兵粮",
            width = 50,
            valueStrGetCall = x => x.food.ToString(),
            valueGetCall = x => x.food,
            valueSortFunc = (a, b) => a.food.CompareTo(b.food),
        };

        public static SortTitle<int> SortByMorale = new SortTitle<int>()
        {
            name = "气力",
            width = 50,
            valueStrGetCall = x => x.morale.ToString(),
            valueGetCall = x => x.morale,
            valueSortFunc = (a, b) => a.morale.CompareTo(b.morale),
        };

        public static SortTitle<int> SortByMoraleByMax = new SortTitle<int>()
        {
            name = "气力",
            width = 50,
            valueStrGetCall = x => $"{x.morale}/{x.MaxMorale}",
            valueGetCall = x => x.morale,
            valueSortFunc = (a, b) => a.morale.CompareTo(b.morale),
        };

        public static SortTitle<int> SortByTroops = new SortTitle<int>()
        {
            name = "士兵",
            width = 50,
            valueStrGetCall = x => x.troops.ToString(),
            valueGetCall = x => x.troops,
            valueSortFunc = (a, b) => a.troops.CompareTo(b.troops),
        };

        public static SortTitle<string> SortByMember1 = new SortTitle<string>()
        {
            name = "副将",
            width = 50,
            valueStrGetCall = x => x.Member1 != null ? x.Member1.Name : "",
            valueGetCall = x => x.Member1 != null ? x.Member1.Name : "",
            valueSortFunc = (a, b) => SangoObject.Compare(a.Member1, b.Member1)
        };

        public static SortTitle<string> SortByMember2 = new SortTitle<string>()
        {
            name = "副将",
            width = 50,
            valueStrGetCall = x => x.Member2 != null ? x.Member2.Name : "",
            valueGetCall = x => x.Member2 != null ? x.Member2.Name : "",
            valueSortFunc = (a, b) => SangoObject.Compare(a.Member2, b.Member2)
        };
    }

}
