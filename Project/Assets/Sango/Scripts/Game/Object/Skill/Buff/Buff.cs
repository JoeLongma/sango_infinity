using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sango.Game.Action;
using System.Collections.Generic;
using System.Numerics;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Buff : SangoObject
    {
        [JsonProperty] public int kind;

        [JsonProperty] public string asset;

        [JsonConverter(typeof(Vector3Converter))]
        [JsonProperty] public UnityEngine.Vector3 offset;

        /// <summary>
        /// 技能效果
        /// </summary>
        [JsonProperty] public JArray buffEffects;

        public bool IsControlBuff()
        {
            return true;
        }
    }
}
