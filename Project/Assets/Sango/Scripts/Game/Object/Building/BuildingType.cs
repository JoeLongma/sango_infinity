using System.IO;
using Newtonsoft.Json;
using System.Xml;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BuildingType : SangoObject
    {
        [JsonProperty] public string desc;
        [JsonProperty] public byte kind;
        [JsonProperty] public string icon;
        [JsonProperty] public int durabilityLimit;
        [JsonProperty] public int buildNumLimit;
        [JsonProperty] public int goldGain;
        [JsonProperty] public int foodGain;
        [JsonProperty] public float populationGain;
        [JsonProperty] public int cost;
        [JsonProperty] public byte radius;
        [JsonProperty] public bool isIntrior;
        [JsonProperty] public string model;
        [JsonProperty] public string modelBroken;

        /// <summary>
        /// 反击攻击力
        /// </summary>
        [JsonProperty] public int atkBack;
        /// <summary>
        /// 攻击力
        /// </summary>
        [JsonProperty] public int atk;
        /// <summary>
        /// 攻击范围
        /// </summary>
        [JsonProperty] public int atkRange;
        
        /// <summary>
        /// 被伤害倍率
        /// </summary>
        [JsonProperty] public float damageBounds;
        
    }
}
