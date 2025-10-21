using UnityEngine;

namespace Sango.Game.Render.Model
{
    public class BuildingBaseModel : MonoBehaviour
    {
        public FlagRender[] flags;

        private void Awake()
        {
            
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
