using UnityEngine;

namespace Sango.Game.Render.Model
{
    public class CityModel : MonoBehaviour
    {
        public FlagRender[] flags;
        public void Init(City city)
        {
            if(city.BelongForce == null)
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
                        flag.Init(city.BelongForce);
                    }
                }
            }
        }
    }
}
