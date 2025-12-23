using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Sango.Game.Action
{
    public class AddBuff : ActionBase
    {
        public override void Clear()
        {
        }

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            Skill skill = sangoObjects[0] as Skill;
            int condition = p.Value<int>("condition");
            int probability = p.Value<int>("prob");
            int buffId = p.Value<int>("buffId");
            int counter = p.Value<int>("counter");
        }
    }
}
