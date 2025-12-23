using Newtonsoft.Json.Linq;
using Sango.Game.Tools;

namespace Sango.Game
{
    public class SkillIsCritical : Condition
    {
        // 比较结果: 0不是 1是
        int result;

        public override bool Check(params object[] objects)
        {
            if (objects.Length < 3) return false;
            for (int i = 0; i < objects.Length; i++)
            {
                Skill skill = objects[i] as Skill;
                if (skill != null)
                {
                    return (skill.tempCriticalFactor > 100) == (result == 1);
                }
            }
            return false;
        }

        public override void Init(JObject p, params SangoObject[] sangoObjects)
        {
            result = p.Value<int>("result");
        }
    }
}
