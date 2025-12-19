using Newtonsoft.Json;

namespace Sango.Game
{
    /*
     * 
     *  剑	0
        枪	1
        刀	1
        弓	1
        军马	2
        冲车	3
        井阑	3
        投石	3
        木兽	3
        走舸	4
        楼船	4
        斗舰	4

     * */
    public enum ItemKindType : int
    {
        None = 0,

        /// <summary>
        /// 兵器
        /// </summary>
        Weapon,

        /// <summary>
        /// 战马
        /// </summary>
        Horse,

        /// <summary>
        /// 器械
        /// </summary>
        Machine,

        /// <summary>
        /// 船
        /// </summary>
        Boat
    }

    public enum ItemSubKindType : int
    {
        None = 0,

        /// <summary>
        /// 兵器
        /// </summary>
        Weapon,

        /// <summary>
        /// 战马
        /// </summary>
        Horse,

        /// <summary>
        /// 器械
        /// </summary>
        Machine,

        /// <summary>
        /// 船
        /// </summary>
        Boat
    }
}
