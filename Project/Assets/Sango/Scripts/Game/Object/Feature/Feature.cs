using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sango.Game
{
    /// <summary>
    /// 特性
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Feature : SangoObject
    {
        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty] public string desc;

        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty] public string kind;

        /// <summary>
        /// 等级
        /// </summary>
        [JsonProperty] public int level;

        /// <summary>
        /// 效果参数
        /// </summary>
        [JsonProperty] public int[][] actionList;

        public void InitActions(List<ActionBase> list, params SangoObject[] sangoObjects)
        {
            if (actionList == null) return;
            for (int i = 0; i < actionList.Length; i++)
            {
                int[] valus = actionList[i];
                ActionBase action = ActionBase.Create(valus[0]);
                action.Init(valus, sangoObjects);
                list.Add(action);
            }
        }

    }
}
