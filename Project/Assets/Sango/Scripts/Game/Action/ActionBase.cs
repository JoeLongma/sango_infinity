using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Sango.Game
{
    public abstract class ActionBase
    {
        public abstract void Init(JObject p, params SangoObject[] sangoObjects);
        public abstract void Clear();

        public delegate ActionBase ActionCreator();

        public static Dictionary<string, ActionCreator> CreateMap = new Dictionary<string, ActionCreator>();
        public static void Register(string name, ActionCreator action)
        {
            CreateMap[name] = action;
        }

        public static ActionBase CraeteHandle<T>() where T : ActionBase, new()
        {
            return new T();
        }
        public static ActionBase Create(string name)
        {
            ActionCreator actionBaseCreator;
            if (CreateMap.TryGetValue(name, out actionBaseCreator))
                return actionBaseCreator();
            return null;
        }

        public static void Init()
        {
            Register("BuildingBaseAttackBack", CraeteHandle<BuildingBaseAttackBack>);
            Register("CityDurabilityLimitAction", CraeteHandle<CityDurabilityLimitAction>);
            Register("CityFoodLimitAction", CraeteHandle<CityFoodLimitAction>);
            Register("CityGoldLimitAction", CraeteHandle<CityGoldLimitAction>);
            Register("CitySecurityChangeAction", CraeteHandle<CitySecurityChangeAction>);
            Register("CityStoreLimitAction", CraeteHandle<CityStoreLimitAction>);
            Register("CityTroopsLimitAction", CraeteHandle<CityTroopsLimitAction>);
            Register("ForceCityMaxMoraleAction", CraeteHandle<ForceCityMaxMoraleAction>);
            Register("ForcePersonLoyaltyChangeAction", CraeteHandle<ForcePersonLoyaltyChangeAction>);
            Register("ForceTroopMaxTroopAction", CraeteHandle<ForceTroopMaxTroopAction>);
            Register("TroopAddAttackAction", CraeteHandle<TroopAddAttackAction>);
            Register("TroopAddDamageBuildingExtraFactorAction", CraeteHandle<TroopAddDamageBuildingExtraFactorAction>);
            Register("TroopAddDamageTroopExtraFactorAction", CraeteHandle<TroopAddDamageTroopExtraFactorAction>);
            Register("TroopAddDefenceAction", CraeteHandle<TroopAddDefenceAction>);
            Register("TroopAddMoveAbilityAction", CraeteHandle<TroopAddMoveAbilityAction>);
            Register("TroopAddSkillAction", CraeteHandle<TroopAddSkillAction>);
            Register("TroopReplaceSkillAction", CraeteHandle<TroopReplaceSkillAction>);
            Register("TroopSkillCalculateCriticalAction", CraeteHandle<TroopSkillCalculateCriticalAction>);
            Register("TroopSkillCalculateSuccessAction", CraeteHandle<TroopSkillCalculateSuccessAction>);
            Register("TroopSkillCalculateAttackBackAction", CraeteHandle<TroopSkillCalculateAttackBackAction>);
            Register("TroopChangeTroopsAction", CraeteHandle<TroopChangeTroopsAction>);
            Register("BuildingImproveFoodGain", CraeteHandle<BuildingImproveFoodGain>);
            Register("BuildingImproveGoldGain", CraeteHandle<BuildingImproveGoldGain>);
            Register("BuildingImproveFoodGainByCityTroops", CraeteHandle<BuildingImproveFoodGainByCityTroops>);
        }

    }
}
