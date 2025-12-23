using Newtonsoft.Json.Linq;
using Sango.Game.Tools;
using System.Collections.Generic;
using UnityEditor;

namespace Sango.Game
{
    public class ConditionAnd : Condition
    {
        Condition L;
        Condition R;

        public override bool Check(params object[] objects)
        {
            if (L != null && !L.Check(objects)) return false;
            if (R != null && !R.Check(objects)) return false;
            return true;
        }

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            JObject Lobj = p.Value<JObject>("L");
            if (Lobj != null)
            {
                L = Condition.Create(Lobj.Value<string>("class"));
                if (L != null)
                    L.Init(Lobj, sangoObjects);
            }

            JObject Robj = p.Value<JObject>("R");
            if (Robj != null)
            {
                R = Condition.Create(Robj.Value<string>("class"));
                if (R != null)
                    R.Init(Robj, sangoObjects);
            }
        }
    }
}
