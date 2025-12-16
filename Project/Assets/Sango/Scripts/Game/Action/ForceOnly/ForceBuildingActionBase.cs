using Sango.Game.Tools;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Sango.Game
{
    public abstract class ForceBuildingActionBase : ForceActionBase
    {
        protected int value;
        protected List<int> kinds;

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);

            value = p.Value<int>("value");
            JArray kindsArray = p.Value<JArray>("kinds");
            if (kindsArray != null)
            {
                kinds = new List<int>(kindsArray.Count);
                for (int i = 0; i < kindsArray.Count; i++)
                    kinds.Add(kindsArray[i].ToObject<int>());
            }
        }

        public virtual bool CheckForceBuilding(BuildingBase buildingBase)
        {
            if (Force != buildingBase.BelongForce) return false;
            if (kinds != null && !kinds.Contains(buildingBase.BuildingType.kind)) return false;
            return true;
        }

    }
}
