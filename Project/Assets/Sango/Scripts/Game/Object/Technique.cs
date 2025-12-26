using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

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
        [JsonProperty] public int needAttr;
        [JsonProperty] public int goldCost;
        [JsonProperty] public int techPointCost;
        [JsonProperty] public int counter;
        [JsonProperty] public int[] needTechs;
        [JsonProperty] public JArray effects;
        [JsonProperty] public int col;
        [JsonProperty] public int row;
        [JsonConverter(typeof(Color32Converter))]
        [JsonProperty] public UnityEngine.Color32 tabColor;

        public bool CanResearch(Force force)
        {
            if (force == null) return false;
            if (force.HasTechnique(Id)) return false;
            if (needTechs != null)
            {
                for (int i = 0; i < needTechs.Length; i++)
                {
                    if (!force.HasTechnique(needTechs[i]))
                        return false;
                }
            }
            return true;
        }

        public bool IsValid(Force force)
        {
            return force.HasTechnique(Id);
        }


    }
}
