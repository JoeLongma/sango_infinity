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
        /// 效果实体集合
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(String2JArrayConverter))]
        public Newtonsoft.Json.Linq.JArray actionEntities;

        public void InitActions(List<ActionBase> list, params SangoObject[] sangoObjects)
        {
            if (actionEntities == null) return;
            for (int i = 0; i < actionEntities.Count; i++)
            {
                JObject valus = actionEntities[i] as JObject;
                ActionBase action = ActionBase.Create(valus.Value<string>("class"));
                if (action != null)
                {
                    action.Init(valus, sangoObjects);
                    list.Add(action);
                }
            }
        }
    }
}
