using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Sango.Game.Action
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
            Register("CityDurabilityLimit", CraeteHandle<CityDurabilityLimit>);
            Register("CityFoodLimit", CraeteHandle<CityFoodLimit>);
            Register("CityGoldLimit", CraeteHandle<CityGoldLimit>);
            Register("CitySecurityChange", CraeteHandle<CitySecurityChange>);
            Register("CityStoreLimit", CraeteHandle<CityStoreLimit>);
            Register("CityTroopsLimit", CraeteHandle<CityTroopsLimit>);
            Register("ForceCityMaxMorale", CraeteHandle<ForceCityMaxMorale>);
            Register("ForcePersonLoyaltyChange", CraeteHandle<ForcePersonLoyaltyChange>);
            Register("ForceTroopMaxTroop", CraeteHandle<ForceTroopMaxTroop>);
            Register("TroopAddAttack", CraeteHandle<TroopAddAttack>);
            Register("TroopAddDamageBuildingExtraFactor", CraeteHandle<TroopAddDamageBuildingExtraFactor>);
            Register("TroopAddDamageTroopExtraFactor", CraeteHandle<TroopAddDamageTroopExtraFactor>);
            Register("TroopAddDefence", CraeteHandle<TroopAddDefence>);
            Register("TroopAddMoveAbility", CraeteHandle<TroopAddMoveAbility>);
            Register("TroopAddSkill", CraeteHandle<TroopAddSkill>);
            Register("TroopReplaceSkill", CraeteHandle<TroopReplaceSkill>);
            Register("TroopSkillCalculateCritical", CraeteHandle<TroopSkillCalculateCritical>);
            Register("TroopSkillCalculateSuccess", CraeteHandle<TroopSkillCalculateSuccess>);
            Register("TroopSkillCalculateAttackBack", CraeteHandle<TroopSkillCalculateAttackBack>);
            Register("TroopChangeTroops", CraeteHandle<TroopChangeTroops>);
            Register("BuildingImproveFoodGain", CraeteHandle<BuildingImproveFoodGain>);
            Register("BuildingImproveGoldGain", CraeteHandle<BuildingImproveGoldGain>);
            Register("BuildingImproveFoodGainByCityTroops", CraeteHandle<BuildingImproveFoodGainByCityTroops>);
            Register("CityImproveJobResult", CraeteHandle<CityImproveJobResult>);
            Register("CityImproveJobCounterResult", CraeteHandle<CityImproveJobCounterResult>);      
        }
    }
}
