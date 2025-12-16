using Sango.Game.Tools;
using Newtonsoft.Json.Linq;

namespace Sango.Game
{
    /// <summary>
    /// 某兵种类型战法的反击率增加
    /// value： 增加值(百分比) kinds： 兵种类型 checkLand： 0:只检查kinds 1:只对landType检查kinds 2只对waterType检查kinds isAttacker： 0攻击方 1受击方 isNormal：  -1都可以 0非 1是 isRange： -1都可以 0非 1是 conditionId： 额外条件 支持参数(troop,troop,skill)
    /// </summary>
    public class TroopSkillCalculateAttackBackAction : TroopTroopActionBase
    {
        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnTroopCalculateAttackBack += OnTroopCalculateAttackBack;
        }

        public override void Clear()
        {
            GameEvent.OnTroopCalculateAttackBack -= OnTroopCalculateAttackBack;
        }

        void OnTroopCalculateAttackBack(Troop attacker, Troop defencer, Skill skill, Scenario scenario, OverrideData<int> overrideData)
        {
            if (!CheckTroop(defencer, attacker, skill)) return;
            overrideData.Value = overrideData.Value * value / 100;
        }
    }
}
