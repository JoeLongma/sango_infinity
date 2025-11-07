using Sango.Game.Tools;

namespace Sango.Game
{
    /// <summary>
    /// 某兵种战法替换成新战法	p1:兵种kind (0全兵种全地形 -1陆地 -2水上)  p2:技能id
    /// </summary>
    public class TroopAddSkillAction : ForceActionBase
    {
        public override void Init(int[] p, params SangoObject[] sangoObjects)
        {
            base.Init(p, sangoObjects);
            GameEvent.OnTroopCalculateAttribute += OnTroopCalculateAttribute;
        }

        public override void Clear()
        {
            GameEvent.OnTroopCalculateAttribute -= OnTroopCalculateAttribute;
        }

        void OnTroopCalculateAttribute(Troop troop, Scenario scenario)
        {
            if (Force == troop.BelongForce && Params.Length > 2)
            {
                int checkTroopTypeKind = Params[1];
                int skillId = Params[2];
                Skill skill = scenario.GetObject<Skill>(skillId);
                if (checkTroopTypeKind == 0)
                {
                    troop.landSkills.Add(new SkillInstance() { Skill = skill });
                    troop.waterSkills.Add(new SkillInstance() { Skill = skill });
                }
                else if (checkTroopTypeKind == -1)
                {
                    troop.landSkills.Add(new SkillInstance() { Skill = skill });
                }
                else if (checkTroopTypeKind == -2)
                {
                    troop.waterSkills.Add(new SkillInstance() { Skill = skill });
                }
                else
                {
                    if (troop.LandTroopType.kind == checkTroopTypeKind)
                    {
                        troop.landSkills.Add(new SkillInstance() { Skill = skill });
                    }
                    if (troop.WaterTroopType.kind == checkTroopTypeKind)
                    {
                        troop.waterSkills.Add(new SkillInstance() { Skill = skill });
                    }
                }

            }
        }
    }
}
