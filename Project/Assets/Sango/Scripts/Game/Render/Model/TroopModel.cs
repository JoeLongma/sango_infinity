using UnityEngine;

namespace Sango.Game.Render.Model
{
    public class TroopModel : MonoBehaviour
    {
        public FlagRender flag;
        public TroopsRender troopsRender;
        public Animation animation;
        public ParticleSystem smoke;

        private void Awake()
        {
            if (animation == null)
                animation = GetComponent<Animation>();

            UnityTools.SetLayer(flag.gameObject, LayerMask.NameToLayer("Flag"));

        }

        public void Init(Troop troop)
        {
            if (flag != null)
                flag.Init(troop);
            SetAniShow(0);
            UpdateTroop(troop);
            if (troopsRender != null)
                troopsRender.UpdateHeight();
        }

        public void UpdateTroop(Troop troop)
        {
            if (troopsRender != null)
                troopsRender.SetShowPercent(Mathf.Clamp01(0.1f + (float)troop.troops / 10000.0f));
        }

        public void SetAniShow(int name)
        {
            if (troopsRender != null)
            {
                troopsRender.SetAniType(name);

            }
            if (name == 1 && animation != null)
                animation.Play("troop_atk_1");
        }

        public void SetSmokeShow(bool b)
        {
            if (troopsRender != null)
            {
                troopsRender.SetSmokeShow(b);
            }
        }
    }
}
