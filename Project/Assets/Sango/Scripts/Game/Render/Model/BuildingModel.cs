using UnityEngine;

namespace Sango.Game.Render.Model
{
    public class BuildingModel : MonoBehaviour
    {
        public FlagRender flag;

        private void Awake()
        {
            
        }

        public void Init(Building building)
        {
            if (flag != null)
                flag.Init(building);
        }
    }
}
