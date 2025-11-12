namespace Sango.Game
{
    public abstract class ActionBase
    {
        public abstract void Init(int[] p, params SangoObject[] sangoObjects);

        public abstract void Clear();

        public static ActionBase Create(int actionType)
        {
            return Create((ActionType)actionType);
        }

        public static ActionBase Create(ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.BuildingBaseAttackBack:
                    return new BuildingBaseAttackBack();
                case ActionType.CityDurabilityLimitAction:
                    return new CityDurabilityLimitAction();
                case ActionType.CityFoodLimitAction:
                    return new CityFoodLimitAction();
                case ActionType.CityGoldLimitAction:
                    return new CityGoldLimitAction();
                case ActionType.CitySecurityChangeAction:
                    return new CitySecurityChangeAction();
                case ActionType.CityStoreLimitAction:
                    return new CityStoreLimitAction();
                case ActionType.CityTroopsLimitAction:
                    return new CityTroopsLimitAction();
                case ActionType.ForceCityMaxMoraleAction:
                    return new ForceCityMaxMoraleAction();
                case ActionType.ForcePersonLoyaltyChangeAction:
                    return new ForcePersonLoyaltyChangeAction();
                case ActionType.ForceTroopMaxTroopAction:
                    return new ForceTroopMaxTroopAction();
                case ActionType.TroopAddAttackAction:
                    return new TroopAddAttackAction();
                case ActionType.TroopAddDamageBuildingExtraFactorAction:
                    return new TroopAddDamageBuildingExtraFactorAction();
                case ActionType.TroopAddDamageTroopExtraFactorAction:
                    return new TroopAddDamageTroopExtraFactorAction();
                case ActionType.TroopAddDefenceAction:
                    return new TroopAddDefenceAction();
                case ActionType.TroopAddMoveAbilityAction:
                    return new TroopAddMoveAbilityAction();
                case ActionType.TroopAddSkillAction:
                    return new TroopAddSkillAction();
                case ActionType.TroopReplaceSkillAction:
                    return new TroopReplaceSkillAction();
                case ActionType.TroopSkillCalculateCriticalAction:
                    return new TroopSkillCalculateCriticalAction();
                case ActionType.TroopSkillCalculateSuccessAction:
                    return new TroopSkillCalculateSuccessAction();
                case ActionType.TroopSkillCalculateAttackBackAction:
                    return new TroopSkillCalculateAttackBackAction();
                case ActionType.TroopChangeTroopsAction:
                    return new TroopChangeTroopsAction();

            }
            return null;
        }
    }
}
