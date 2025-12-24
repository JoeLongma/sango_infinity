using Newtonsoft.Json.Linq;
using Sango.Game.Tools;
using System.Collections.Generic;

namespace Sango.Game
{
    public class ProbabilityCheck : Condition
    {
        int probability;
        public override bool Check(params object[] objects)
        {
            return GameRandom.Range(0, 10000) <= probability;
        }

        public override bool Check(Troop troop, Troop target, Skill skill)
        {
            return GameRandom.Range(0, 10000) <= probability;
        }
        public override bool Check(SkillInstance skillInstance, Troop troop, Cell spellCell, List<Cell> atkCellList)
        {
            return GameRandom.Range(0, 10000) <= probability;
        }

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            probability = p.Value<int>("probability");
        }
    }
}
