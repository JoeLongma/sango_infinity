namespace Sango.Game
{
    public abstract class TroopActionBase : ActionBase
    {
        protected Force Force { get; set; }
        protected Troop Troop { get; set; }
        protected int[] Params { get; set; }
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            Troop = sangoObjects[0] as Troop;
            if(Troop == null)
                Force = sangoObjects[0] as Force;
            Params = p;
        }

        /// <summary>
        /// 兵种类型(0全兵种全地形 -1陆地 -2水上)
        /// </summary>
        /// <param name="troop"></param>
        /// <param name="checkTroopTypeKind"></param>
        /// <returns></returns>
        public bool CheckTroopTypeKind(Troop troop, int checkTroopTypeKind)
        {
            if (checkTroopTypeKind == -1 && troop.IsInWater)
                return false;
            if (checkTroopTypeKind == -2 && !troop.IsInWater)
                return false;

            if (checkTroopTypeKind > 0 && ((troop.LandTroopType.kind == checkTroopTypeKind && troop.IsInWater) || (troop.WaterTroopType.kind == checkTroopTypeKind && !troop.IsInWater)))
                return false;
            return true;
        }

        /// <summary>
        /// 是否一般攻击 1一般攻击 0非一般攻击 -1都可以
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="isNormalSkill"></param>
        /// <returns></returns>
        public bool CheckIsNormalSkill(Skill skill, int isNormalSkill)
        {
            if ((isNormalSkill == 1 && skill.costEnergy > 0) || (isNormalSkill == 0 && skill.costEnergy == 0))
                return false;
            return true;
        }

        /// <summary>
        /// 是否是远程 1远程 0近战 -1都可以 
        /// </summary>
        /// <param name="skill"></param>
        /// <param name="isRangeSkill"></param>
        /// <returns></returns>
        public bool CheckIsRangeSkill(Skill skill, int isRangeSkill)
        {
            if ((isRangeSkill == 1 && !skill.IsRange()) || (isRangeSkill == 0 && skill.IsRange()))
                return false;
            return true;
        }

    }
}
