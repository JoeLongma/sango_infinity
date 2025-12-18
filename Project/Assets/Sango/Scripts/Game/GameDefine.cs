namespace Sango.Game
{
    public enum SeasonType : int
    {
        Autumn = 0,
        Spring = 1,
        Summer = 2,
        Winter = 3,
    }

    public class GameDefine
    {
        //--- 每个月对应的季节
        public static SeasonType[] SeasonInMonth = {
            SeasonType.Spring,
            SeasonType.Spring,
            SeasonType.Spring,
            SeasonType.Summer,
            SeasonType.Summer,
            SeasonType.Summer,
            SeasonType.Autumn,
            SeasonType.Autumn,
            SeasonType.Autumn,
            SeasonType.Winter,
            SeasonType.Winter,
            SeasonType.Winter,
            };

    }


    /// <summary>
    /// 对象枚举类型：武将性别（男、女）
    /// </summary>
    public enum Sex : int
    {
        //男
        Male = 0,
        //女
        Female = 1
    }

    /// <summary>
    /// 对象枚举类型：武将适性级别（C、B、A、S、SS、SSR）
    /// </summary>
    public enum LevelString : int
    {
        Ｃ = 0,
        Ｂ,
        Ａ,
        Ｓ,
        ＳＳ,
        ＳＳＳ,
        ＳＳR,
    }

    //public string ox = "○×";

    /// <summary>
    /// 对象枚举类型：宝物类型
    /// </summary>
    public enum TreasureType : int
    {
        //-@马
        horse = 0,
        //-@武器
        Weapon = 1,
        //-@书
        Book = 2,
    }

    public enum CityJobType : int
    {
        Default = 0,

        /// <summary>
        /// 农业
        /// </summary>
        Farming = 1,

        /// <summary>
        /// 商业
        /// </summary>
        Develop = 2,

        /// <summary>
        /// 巡查
        /// </summary>
        Inspection = 3,

        /// <summary>
        /// 训练
        /// </summary>
        TrainTroops = 4,

        /// <summary>
        /// 搜索
        /// </summary>
        Searching = 5,

        /// <summary>
        /// 招募士兵
        /// </summary>
        RecuritTroops = 6,

        /// <summary>
        /// 招募武将
        /// </summary>
        RecuritPerson = 7,

        /// <summary>
        /// 生产兵装
        /// </summary>
        CreateItems = 8,

        /// <summary>
        /// 建造
        /// </summary>
        Build = 9,

        /// <summary>
        /// 生产兵器
        /// </summary>
        CreateMachine = 10,

        /// <summary>
        /// 生产船
        /// </summary>
        CreateBoat = 11,

        /// <summary>
        /// 生产马
        /// </summary>
        CreateHourse = 12,

        /// <summary>
        /// 交易粮食
        /// </summary>
        TradeFood = 13,

        /// <summary>
        /// 派遣武将
        /// </summary>
        TransformPerson = 14,

        /// <summary>
        /// 召唤武将
        /// </summary>
        CallPerson = 15,

        /// <summary>
        /// 升级建筑
        /// </summary>
        UpgradeBuilding = 16,

        /// <summary>
        /// 组建部队
        /// </summary>
        MakeTroop = 17,

        /// <summary>
        /// 组建运输部队
        /// </summary>
        MakeTansport = 18,

        /// <summary>
        /// 研究
        /// </summary>
        Research = 19,

        MaxJobCount
    }

    public enum InfoType : int
    {
        Gold = 0,
        Spear,
        Troop,
        Sword,
        CrossBow,
        Food,
        Hourse,
        /// <summary>
        /// 冲车
        /// </summary>
        Helepolis,

        /// <summary>
        /// 木兽
        /// </summary>
        WoodenBeast,

        /// <summary>
        /// 井阑
        /// </summary>
        SiegeTower,

        /// <summary>
        /// 投石车
        /// </summary>
        Catapult,
        Boat,
        BigBoat,
        Morale,
        Durability,
        Security
    }
}
