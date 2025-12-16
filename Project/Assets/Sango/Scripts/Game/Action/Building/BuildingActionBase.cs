using Newtonsoft.Json.Linq;

namespace Sango.Game
{
    public abstract class BuildingActionBase : ActionBase
    {
        protected Force Force { get; set; }
        public BuildingBase Building { get; set; }
        protected JObject Params { get; set; }

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            Building = sangoObjects[0] as BuildingBase;
            if (Building == null)
                Force = sangoObjects[0] as Force;
            Params = p;
        }
    }
}
