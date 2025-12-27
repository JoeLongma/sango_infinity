namespace Sango.Game
{
    public enum MissionType : int
    {
        None = 0,
        TroopReturnCity,
        TroopMovetoCity,
        TroopDestroyTroop,
        TroopDestroyBuilding,
        TroopOccupyCity,
        TroopHarassCity,
        TroopBanishTroop,
        TroopProtectBuilding,
        TroopProtectTroop,
        TroopProtectCity,
        TroopBuildBuilding,
        TroopTransformGoodsToCity,


        PersonBuild,
        PersonCreateBoat,
        PersonCreateMachine,
        PersonWork,
        PersonInTroop,
        PersonInCityCaptive,
        PersonInTroopCaptive,
        PersonResearch,

        /// <summary>
        /// 移动
        /// </summary>
        PersonTransform,

        /// <summary>
        /// 返回所在城市
        /// </summary>
        PersonReturn,

        /// <summary>
        /// 招募
        /// </summary>
        PersonRecruitPerson

    }
}
