namespace Sango.Game
{
    public class TroopSortFunction : Singleton<TroopSortFunction>
    {
        public delegate string TroopValueStrGet(Troop troop);
        public delegate T TroopValueGet<T>(Troop troop);
        public delegate int TroopSortFunc(Troop troop1, Troop troop2);

        public class SortTitle<T> : ObjectSortTitle
        {
            public TroopValueStrGet valueGetStrCall;
            public TroopSortFunc personSortFunc;
            public TroopValueGet<T> valueGetCall;

            public  T GetValue(SangoObject obj)
            {
                return valueGetCall.Invoke((Troop)obj);
            }

            public override string GetValueStr(SangoObject obj)
            {
                return valueGetStrCall.Invoke((Troop)obj);
            }

            public override int Sort(SangoObject a, SangoObject b)
            {
                return personSortFunc.Invoke((Troop)a, (Troop)b);
            }
        }

        public static SortTitle<string> SortByName = new SortTitle<string>()
        {
            name = "武将",
            width = 80,
            valueGetCall = x => x.Name,
            personSortFunc = (a, b) => a.Name.CompareTo(b.Name),
        };

        public static SortTitle<int> SortByDefence = new SortTitle<int>()
        {
            name = "防御",
            width = 50,
            valueGetStrCall = x => x.Defence.ToString(),
            valueGetCall = x => x.Defence,
            personSortFunc = (a, b) => a.Defence.CompareTo(b.Defence),
        };

        public static SortTitle<int> SortByAttack = new SortTitle<int>()
        {
            name = "攻击",
            width = 50,
            valueGetStrCall = x => x.Attack.ToString(),
            valueGetCall = x => x.Attack,
            personSortFunc = (a, b) => a.Attack.CompareTo(b.Attack),
        };

        public static SortTitle<int> SortByMoveability = new SortTitle<int>()
        {
            name = "移动",
            width = 50,
            valueGetStrCall = x => x.MoveAbility.ToString(),
            valueGetCall = x => x.MoveAbility,
            personSortFunc = (a, b) => a.MoveAbility.CompareTo(b.MoveAbility),
        };

        public static SortTitle<int> SortByBuild = new SortTitle<int>()
        {
            name = "建设",
            width = 50,
            valueGetStrCall = x => x.BuildPower.ToString(),
            valueGetCall = x => x.BuildPower,
            personSortFunc = (a, b) => a.BuildPower.CompareTo(b.BuildPower),
        };

        public static SortTitle<int> SortByIntelligence = new SortTitle<int>()
        {
            name = "智力",
            width = 50,
            valueGetStrCall = x => x.Intelligence.ToString(),
            valueGetCall = x => x.Intelligence,
            personSortFunc = (a, b) => a.Intelligence.CompareTo(b.Intelligence),
        };
    }
}
