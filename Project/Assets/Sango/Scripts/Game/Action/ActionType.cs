using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Sango.Game
{
    public enum ActionType : int
    {
        None = 0,

        ForceTroopMaxTroopAction,
        ForceCityMaxMoraleAction,
        BuildingBaseAttackBack,
        CityGoldLimitAction,
        CityFoodLimitAction,
        CityTroopsLimitAction,
        CityStoreLimitAction,
        CityDurabilityLimitAction,

        CitySecurityChangeAction,
        ForcePersonLoyaltyChangeAction,
        TroopAddAttackAction,
        TroopAddDefenceAction,
        TroopAddMoveAbilityAction,

        TroopAddDamageTroopExtraFactorAction,
        TroopAddDamageBuildingExtraFactorAction,
        TroopReplaceSkillAction,
        TroopAddSkillAction,
        TroopSkillCalculateSuccessAction,
        TroopSkillCalculateCriticalAction,
    }
}
