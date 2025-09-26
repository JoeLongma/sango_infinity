using UnityEngine;

namespace Sango.Game.Render.Model
{
    public class TroopModel : MonoBehaviour
    {
        public FlagRender flag;
        public TroopsRender troopsRender;

        public void Init(Troop troop)
        {
            if (flag != null)
                flag.Init(troop);

            UpdateTroop(troop);
        }

        public void UpdateTroop(Troop troop)
        {
            if (troopsRender != null)
                troopsRender.SetShowPercent(Mathf.Clamp01(0.3f + troop.troops / 10000.0f));

        }
    }
}
