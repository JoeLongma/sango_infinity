using Newtonsoft.Json.Linq;

namespace Sango.Game
{
    public abstract class CityActionBase : ActionBase
    {
        protected Force Force { get; set; }
        protected JObject Params { get; set; }

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            Force = sangoObjects[0] as Force;
            Params = p;
        }
    }
}
