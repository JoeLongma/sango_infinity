using Sango.Game.Tools;

namespace Sango.Game
{
    public class SkillIsCritical : Condition
    {
        public override bool Check(params object[] objects)
        {
            if (objects.Length < 3) return false;
            Skill skill = objects[2] as Skill;
            if (skill.costEnergy == 0)
                return false;

            return skill.tempCriticalFactor > 100;
        }
    }
}
