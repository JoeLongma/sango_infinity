using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Sango.Game
{
    public abstract class Condition
    {
        public abstract void Init(JObject p, params SangoObject[] sangoObjects);
        public abstract bool Check(params object[] objects);
        public virtual bool Check(Troop troop, Troop target, Skill skill) { return false; }
        public virtual bool Check(SkillInstance skillInstance, Troop troop, Cell spellCell, List<Cell> atkCellList) { return false; }

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
            Register("and", CraeteHandle<ConditionAnd>);
            Register("or", CraeteHandle<ConditionOr>);
            Register("list", CraeteHandle<ConditionList>);

            // Troop
            Register("TroopAttributeCompare", CraeteHandle<TroopAttributeCompare>);

            // Skill
            Register("SkillIsCritical", CraeteHandle<SkillIsCritical>);
            Register("SkillIsNormalSkill", CraeteHandle<SkillIsNormalSkill>);

        }
    }
}
