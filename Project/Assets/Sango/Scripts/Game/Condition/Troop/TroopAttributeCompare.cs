using Newtonsoft.Json.Linq;
using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 部队属性比较
    /// attType: 部队属性类型
    /// result: 比较结果 0等于 1大于 -1小于
    /// </summary>
    public class TroopAttributeCompare : Condition
    {
        // 部队属性类型
        string attType;

        // 比较结果: 0等于 1大于 -1小于
        int result;

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            attType = p.Value<string>("attType");
            result = p.Value<int>("result");
        }

        public override bool Check(params object[] objects)
        {
            if (objects.Length < 3) return false;
            Troop atker = objects[0] as Troop;
            Troop target = objects[1] as Troop;
            if (target == null) return true;
            TroopCompareFunction.TroopCompare compareFunction = TroopCompareFunction.Instance.Get(attType);
            if (compareFunction == null)
            {
                Sango.Log.Error($"TroopCompareFunction中没有找到{attType}的函数");
                return false;
            }

            return compareFunction(atker, target) == result;
        }
    }
}
