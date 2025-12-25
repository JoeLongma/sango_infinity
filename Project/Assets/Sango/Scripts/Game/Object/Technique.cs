using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Sango.Game
{
    /// <summary>
    /// 州
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Technique : SangoObject
    {
        [JsonProperty] public string desc;
        [JsonProperty] public string kind;
        [JsonProperty] public int level;
        [JsonProperty] public int order;
        [JsonProperty] public int goldCost;
        [JsonProperty] public int techPointCost;
        [JsonProperty] public int counter;
        [JsonProperty] public int[] needTechs;
        [JsonProperty] public JArray effects;
    }
}
