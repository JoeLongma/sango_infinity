using Sango.Game.Render;
using Sango.Game.Render.UI;
using UnityEngine;
using Sango.Game;

namespace Sango.Game.Render.Model
{
    public class CityModel : MonoBehaviour
    {
        public FlagRender[] flags;
        public void Init(Force force)
        {
            if(force == null)
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
                        flag.Init(force);
                    }
                }
            }
        }
    }
}
