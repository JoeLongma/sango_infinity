using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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

        public override bool Check(Troop troop, Troop target, Skill skill)
        {
            if (L != null && !L.Check(troop, target, skill)) return false;
            if (R != null && !R.Check(troop, target, skill)) return false;
            return true;
        }

        public override bool Check(SkillInstance skillInstance, Troop troop, Cell spellCell, List<Cell> atkCellList)
        {
            if (L != null && !L.Check(skillInstance, troop, spellCell, atkCellList)) return false;
            if (R != null && !R.Check(skillInstance, troop, spellCell, atkCellList)) return false;
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
