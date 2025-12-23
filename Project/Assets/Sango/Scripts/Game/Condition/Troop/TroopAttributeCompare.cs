using Sango.Game.Tools;

namespace Sango.Game
{
    public class TroopAttributeCompare : Condition
    {
        public override bool Check(params object[] objects)
        {
            if (objects.Length < 3) return false;
            Skill skill = objects[2] as Skill;
            if(skill.costEnergy == 0)
            {
                return false;
            }
            Troop atker = objects[0] as Troop;
            Troop target = objects[1] as Troop;
            if (target == null) return true;
            return atker.Strength > target.Strength;
        }
    }
}
