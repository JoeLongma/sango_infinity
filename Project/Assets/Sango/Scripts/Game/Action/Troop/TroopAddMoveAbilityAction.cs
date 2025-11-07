using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 某兵种攻击力增加	p1:增加值 p2:兵种kind (0全兵种全地形 -1陆地 -2水上)
    /// </summary>
    public class TroopAddMoveAbilityAction : TroopActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnTroopCalculateAttribute += OnTroopCalculateAttribute;
        }

        public override void Clear()
        {
            GameEvent.OnTroopCalculateAttribute -= OnTroopCalculateAttribute;
        }

        void OnTroopCalculateAttribute(Troop troop, Scenario scenario)
        {
            if (Force != null && troop.BelongForce != Force) return;
            if (Troop != null && Troop != troop) return;

            if (Params.Length > 2)
            {
                int checkTroopTypeKind = Params[2];
                if (checkTroopTypeKind == 0)
                {
                    troop.landMoveAbility += Params[1];
                    troop.waterMoveAbility += Params[1];
                }
                else if (checkTroopTypeKind == -1)
                {
                    troop.landMoveAbility += Params[1];
                }
                else if (checkTroopTypeKind == -2)
                {
                    troop.waterMoveAbility += Params[1];
                }
                else
                {
                    if (troop.LandTroopType.kind == checkTroopTypeKind)
                        troop.landMoveAbility += Params[1];
                    if (troop.WaterTroopType.kind == checkTroopTypeKind)
                        troop.waterMoveAbility += Params[1];
                }
            }
        }
    }
}
