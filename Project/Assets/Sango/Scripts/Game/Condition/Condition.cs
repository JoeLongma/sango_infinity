using Newtonsoft.Json.Linq;
using Sango.Game.Tools;
using System.Collections.Generic;
using UnityEditor;

namespace Sango.Game
{
    public abstract class Condition
    {
        public abstract void Init(JObject p, params SangoObject[] sangoObjects);
        public abstract bool Check(params object[] objects);

        public delegate Condition ConditionCreator();

        public static Dictionary<string, ConditionCreator> CreateMap = new Dictionary<string, ConditionCreator>();
        public static void Register(string name, ConditionCreator action)
        {
            CreateMap[name] = action;
        }

        public static Condition CraeteHandle<T>() where T : Condition, new()
        {
            return new T();
        }
        public static Condition Create(string name)
        {
            ConditionCreator actionBaseCreator;
            if (CreateMap.TryGetValue(name, out actionBaseCreator))
                return actionBaseCreator();
            return null;
        }

        public static void Init()
        {
            // core
            Register("ConditionAnd", CraeteHandle<ConditionAnd>);
            Register("ConditionOr", CraeteHandle<ConditionOr>);
            Register("ConditionList", CraeteHandle<ConditionList>);

            // Troop
            Register("TroopAttributeCompare", CraeteHandle<TroopAttributeCompare>);

            // Skill
            Register("SkillIsCritical", CraeteHandle<SkillIsCritical>);
            Register("SkillIsNormalSkill", CraeteHandle<SkillIsNormalSkill>);

        }
    }
}
