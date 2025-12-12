using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Collections.Generic;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BuildingType : SangoObject
    {
        [JsonProperty] public string desc;
        [JsonProperty] public byte majorType;
        [JsonProperty] public byte kind;
        [JsonProperty] public int nextId;
        [JsonProperty] public int level;
        [JsonProperty] public string icon;
        [JsonProperty] public int durabilityLimit;
        [JsonProperty] public int buildNumLimit;
        [JsonProperty] public int goldGain;
        [JsonProperty] public int foodGain;
        [JsonProperty] public float populationGain;
        [JsonProperty] public int cost;
        [JsonProperty] public byte radius;
        //[JsonProperty] public bool IsIntrior;
        //[JsonProperty] public bool IsOutside;
        [JsonProperty] public string model;
        [JsonProperty] public string modelBroken;
        [JsonProperty] public string modelCreate;
        [JsonProperty] public bool canFire;
        [JsonProperty] public short techGain;

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
        
        public bool CanBuildToHere(Cell cell)
        {
            return true;
        }

        public bool IsValid(Force force)
        {
            return true;
        }

        public bool IsIntrior => majorType == (int)BuildingMajorType.Interior;
        public bool IsOutside => IsMilitary || IsExplosive || IsObstacle;
        public bool IsMilitary => majorType == (int)BuildingMajorType.Military;
        public bool IsExplosive => majorType == (int)BuildingMajorType.Explosive;
        public bool IsObstacle => majorType == (int)BuildingMajorType.Obstacle;

        public Cell GetBestPlace(City city)
        {
            return city.GetEmptyInteriorCell();
        }

    }
}
