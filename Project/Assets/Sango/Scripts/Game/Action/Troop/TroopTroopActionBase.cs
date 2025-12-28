using Sango.Game.Tools;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Sango.Game.Action
{
    /// <summary>
    /// 某兵种类型战法的增减伤害  p1:0攻击方 1受击方 p2:兵种类型(0全兵种全地形 -1陆地 -2水上) p3:增加值(百分比) p4: 是否一般攻击 -1都可以 p5: 是否是远程 -1都可以 p6:其他条件(troop,troop,skill)
    /// value 增加值(百分比)
    /// kinds 兵种类型
    /// checkLand 0:只检查kinds 1:只对landType检查kinds 2只对waterType检查kinds
    /// isAttacker 0受击方 1攻击方
    /// isNormal  -1都可以 0非 1是
    /// isRange -1都可以 0非 1是
    /// condition 额外条件 支持参数(troop,troop,skill)
    /// </summary>
    public abstract class TroopTroopActionBase : TroopActionBase
    {
        protected int checkLand;
        protected int isAttacker;
        protected int isNormal;
        protected int isRange;
        protected Condition condition;

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            checkLand = p.Value<int>("checkLand");
            isAttacker = p.Value<int>("isAttacker");
            isNormal = p.Value<int>("isNormal");
            isRange = p.Value<int>("isRange");
            JObject conObj = p.Value<JObject>("condition");
            if (conObj != null)
            {
                condition = Condition.Create(conObj.Value<string>("class"));
                condition.Init(conObj, sangoObjects);
            }
        }

        protected virtual bool CheckTroop(Troop defencer, SangoObject atker, Skill skill)
        {
            Troop troop = defencer;
            if (isAttacker == 1)
            {
                if (atker.ObjectType != SangoObjectType.Troops) return false;

                troop = (Troop)atker;
            }
            if (Force != null && troop.BelongForce != Force) return false;
            if (Troop != null && Troop != troop) return false;

            if (!CheckIsNormalSkill(skill, isNormal))
                return false;

            if (!CheckIsRangeSkill(skill, isRange))
                return false;

            if (checkLand == 1 && troop.IsInWater)
                return false;
            if (checkLand == 2 && !troop.IsInWater)
                return false;

            if (checkLand == 0 && kinds != null && !kinds.Contains(troop.LandTroopType.kind) && !kinds.Contains(troop.WaterTroopType.kind))
                return false;

            if (checkLand == 1 && kinds != null && !kinds.Contains(troop.LandTroopType.kind))
                return false;

            if (checkLand == 2 && kinds != null && !kinds.Contains(troop.WaterTroopType.kind))
                return false;

            if (condition != null && !condition.Check(troop, defencer, skill))
                return false;

            return true;
        }
    }
}
