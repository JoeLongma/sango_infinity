using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Sango.Game
{
    public class ConditionList : Condition
    {
        List<Condition> Conditions = new List<Condition>();

        public override bool Check(params object[] objects)
        {
            for (int i = 0; i < Conditions.Count; ++i)
            {
                Condition c = Conditions[i];
                if (c != null && !c.Check(objects))
                    return false;
            }
            return true;
        }

        public override bool Check(Troop troop, Troop target, Skill skill)
        {
            for (int i = 0; i < Conditions.Count; ++i)
            {
                Condition c = Conditions[i];
                if (c != null && !c.Check(troop, target, skill))
                    return false;
            }
            return true;
        }

        public override bool Check(SkillInstance skillInstance, Troop troop, Cell spellCell, List<Cell> atkCellList)
        {
            for (int i = 0; i < Conditions.Count; ++i)
            {
                Condition c = Conditions[i];
                if (c != null && !c.Check(skillInstance, troop, spellCell, atkCellList))
                    return false;
            }
            return true;
        }

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            JArray array = p.Value<JArray>("list");
            for (int i = 0; i < array.Count; i++)
            {
                JObject obj = array[i].Value<JObject>();
                Condition c = Condition.Create(obj.Value<string>("class"));
                if (c != null)
                {
                    Conditions.Add(c);
                }
            }
        }
    }
}
