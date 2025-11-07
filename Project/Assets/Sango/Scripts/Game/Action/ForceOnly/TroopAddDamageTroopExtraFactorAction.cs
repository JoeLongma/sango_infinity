using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 某兵种战法威力(百分比)增加	p1:兵种kind p2:增加值(百分比)
    /// </summary>
    public class TroopAddDamageTroopExtraFactorAction : ForceActionBase
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
            if (Force == troop.BelongForce && Params.Length > 2)
            {
                float factor = Params[1] / 100f;
                int checkTroopTypeKind = Params[2];
                if (checkTroopTypeKind == 0)
                {
                    troop.landDamageTroopExtraFactor += factor;
                    troop.waterDamageTroopExtraFactor += factor;
                }
                else
                {
                    if (troop.LandTroopType.kind == checkTroopTypeKind)
                        troop.landDamageTroopExtraFactor += factor;
                    if (troop.WaterTroopType.kind == checkTroopTypeKind)
                        troop.waterDamageTroopExtraFactor += factor;
                }
            }
        }
    }
}
