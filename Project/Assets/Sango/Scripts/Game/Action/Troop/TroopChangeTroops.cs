using Sango.Game.Tools;
using Newtonsoft.Json.Linq;

namespace Sango.Game.Action
{
    /// <summary>
    /// 某兵种类型战法的增减伤害  
    /// value： 增加值(百分比) kinds： 兵种类型 checkLand： 0:只检查kinds 1:只对landType检查kinds 2只对waterType检查kinds isAttacker： 0攻击方 1受击方 isNormal：  -1都可以 0非 1是 isRange： -1都可以 0非 1是 condition： 额外条件 支持参数(troop,troop,skill)
    /// </summary>
    public class TroopChangeTroops : TroopTroopActionBase
    {

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnTroopChangeTroops += OnTroopChangeTroops;
        }

        public override void Clear()
        {
            GameEvent.OnTroopChangeTroops -= OnTroopChangeTroops;
        }

        void OnTroopChangeTroops(Troop defencer, SangoObject atker, Skill skill, int atkBack, OverrideData<int> overrideData)
        {
            if (!CheckTroop(defencer, atker, skill)) return;
            overrideData.Value = overrideData.Value * value / 100;
        }
    }
}
