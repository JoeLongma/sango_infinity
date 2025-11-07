using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 势力忠诚减少概率  p1:修改值(百分比)
    /// </summary>
    public class ForcePersonLoyaltyChangeAction : ForceActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnForcePersonLoyaltyChangeProbability += OnForcePersonLoyaltyChangeProbability;
        }

        public override void Clear()
        {
            GameEvent.OnForcePersonLoyaltyChangeProbability -= OnForcePersonLoyaltyChangeProbability;
        }

        void OnForcePersonLoyaltyChangeProbability(Force force, OverrideData<int> overrideData)
        {
            if (Force == force && Params.Length > 1)
            {
                overrideData.Value = (int)System.Math.Ceiling(overrideData.Value * Params[1] / 100f);
            }
        }
    }
}
