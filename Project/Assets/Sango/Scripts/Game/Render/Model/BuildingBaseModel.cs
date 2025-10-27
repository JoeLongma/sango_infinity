using UnityEngine;

namespace Sango.Game.Render.Model
{
    public class BuildingBaseModel : MonoBehaviour
    {
        public FlagRender[] flags;

        protected virtual void Awake()
        {
            for(int i = 0; i < flags.Length; i++)
            {
                UnityTools.SetLayer(flags[i].gameObject, LayerMask.NameToLayer("Flag"));
            }

        }

        public void Init(BuildingBase building)
        {
            if (building.BelongForce == null)
            {
                foreach (FlagRender flag in flags)
                {
                    if (flag != null)
                    {
                        flag.gameObject.SetActive(false);
                    }
                }
                return;
            }

            if (flags != null)
            {
                foreach (FlagRender flag in flags)
                {
                    if (flag != null)
                    {
                        flag.gameObject.SetActive(true);
                        flag.Init(building.BelongForce);
                    }
                }
            }
        }
    }
}
