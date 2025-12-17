using Newtonsoft.Json.Linq;

namespace Sango.Game.Action
{
    public abstract class CityActionBase : ActionBase
    {
        protected City City { get; set; }
        protected Force Force { get; set; }
        protected JObject Params { get; set; }

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            City = sangoObjects[0] as City;
            if(City == null)
            {
                Force = sangoObjects[0] as Force;
            }
            Params = p;
        }
    }
}
