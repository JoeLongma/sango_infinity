using Sango.Game.Render;
using Sango.Game.Render.UI;
using Sango.Render;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class TroopCommandSkill : TroopCommandAttack
    {
        public TroopCommandSkill()
        {
            customMenuName = "战法";
            customMenuOrder = 0;
        }

        protected override void OnTroopActionContextMenuShow(ContextMenuData menuData, Troop troop, Cell actionCell)
        {
            if (troop.BelongForce != null && troop.BelongForce.IsPlayer && troop.BelongForce == Scenario.Cur.CurRunForce)
            {
                TargetTroop = troop;
                ActionCell = actionCell;

                List<SkillInstance> list;
                if (ActionCell.TerrainType.isWater)
                    list = TargetTroop.waterSkills;
                else
                    list = TargetTroop.landSkills;
                for (int i = 0, count = list.Count; i < count; ++i)
                {
                    Skill skill = list[i].Skill;
                    if (skill.costEnergy > 0)
                    {
                        bool isValid = skill.CanBeSpell(TargetTroop);
                        if(isValid)
                        {
                            isValid = false;
                            spellRangeCell.Clear();
                            skill.GetSpellRange(TargetTroop, ActionCell, spellRangeCell);
                            foreach (Cell c in spellRangeCell)
                            {
                                if (skill.CanSpeellToHere(TargetTroop, c))
                                {
                                    isValid = true;
                                    break;
                                }
                            }
                        }
                        menuData.Add($"战法/{skill.Name}({skill.costEnergy})", skill.costEnergy, skill, OnClickMenuItem, isValid);
                    }
                }
            }
        }


        protected override void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            Start(TargetTroop, ActionCell, contextMenuItem.customData as Skill);
        }

        public void Start(Troop troop, Cell actionCell, Skill skill)
        {
            spellSkill = skill;
            base.Start(troop, actionCell);
        }

        public override void OnEnter()
        {
            isShow = false;
            isMoving = false;
            spellRangeCell.Clear();
            ContextMenu.SetVisible(false);
            MovePath = Singleton<TroopSystem>.Instance.movePath;
            Cell stayCell = ActionCell;
            if (spellSkill.CanBeSpell(TargetTroop))
            {
                List<Cell> rangeCell = new List<Cell>();
                spellSkill.GetSpellRange(TargetTroop, stayCell, rangeCell);
                foreach (Cell c in rangeCell)
                {
                    if (spellSkill.CanSpeellToHere(TargetTroop, c))
                    {
                        spellRangeCell.Add(c);
                    }
                }
            }
            ShowSpellRange();
        }
    }
}
