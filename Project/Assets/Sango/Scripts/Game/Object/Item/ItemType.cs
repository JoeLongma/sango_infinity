using Newtonsoft.Json;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]

    public class ItemType : SangoObject
    {
        /// <summary>
        /// 主类型
        /// </summary>
        [JsonProperty] public byte kind;

        /// <summary>
        /// 次类型
        /// </summary>
        [JsonProperty] public byte subKind;

        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty] public string desc;

        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty] public string icon;

        /// <summary>
        /// 额外费用
        /// </summary>
        [JsonProperty] public int cost;

        /// <summary>
        /// 所需科技
        /// </summary>
        [JsonProperty] public int needTechnique;

        /// <summary>
        /// 额外参数1,
        /// </summary>
        [JsonProperty] public int p1;

        /// <summary>
        /// 额外参数2,
        /// </summary>
        [JsonProperty] public int p2;

        /// <summary>
        /// 额外参数3,
        /// </summary>
        [JsonProperty] public int p3;


        public bool IsValid(Force force)
        {
            if (needTechnique == 0) return true;
            return force.HasTechnique(needTechnique);
        }
    }
}
