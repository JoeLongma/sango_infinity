using Sango.Game.Tools;
using System.Collections.Generic;
using UnityEditor;

namespace Sango.Game
{
    public class Condition
    {
        public static Condition Default = new Condition();
        public virtual bool Check(params object[] objects)
        {
            return false;
        }

        static Dictionary<ConditionType, Condition> ConditionMap = new Dictionary<ConditionType, Condition>();

        public static Condition Get(int conditionType)
        {
            return Get((ConditionType)conditionType);
        }

        public static Condition Get(ConditionType conditionType)
        {
            Condition condition;
            if(ConditionMap.TryGetValue(conditionType, out condition))
                return condition;

            condition = Create(conditionType);
            ConditionMap.Add(conditionType, condition);
            return condition;
        }

        public static Condition Create(ConditionType conditionType)
        {
            switch(conditionType)
            {
                case ConditionType.TroopIntelligenceGreaterThanTarget:
                    return new TroopIntelligenceGreaterThanTarget();
                case ConditionType.TroopStrengthGreaterThanTarget:
                    return new TroopStrengthGreaterThanTarget();
            }

            return Default;
        }
    }
}
