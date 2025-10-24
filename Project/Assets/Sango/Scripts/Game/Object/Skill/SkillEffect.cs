using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SkillEffect : SangoObject
    {
        [JsonProperty]
        public int effectType;
        [JsonProperty]
        public int p1;
        [JsonProperty]
        public int p2;
        [JsonProperty]
        public int p3;
        [JsonProperty]
        public int p4;
        [JsonProperty]
        public int p5;
        [JsonProperty]
        public int p6;
        [JsonProperty]
        public int p7;
        [JsonProperty]
        public int p8;
        [JsonProperty]
        public int p9;
        [JsonProperty]
        public int p10;

        public static SkillEffect Create(int effectType)
        {
            SkillEffectType skillEffectType = (SkillEffectType)effectType;
            switch (skillEffectType)
            {
                case SkillEffectType.None:
                    return new SkillEffect();
                case SkillEffectType.BurnSomeWhere:
                    return new BurnSomeWhere();
                default:
                    return new SkillEffect();
            }
        }

        public virtual void Action(SkillInstance skillInstance, Troop troop, Cell spellCell, List<Cell> atkCellList)
        {

        }


    }
}
