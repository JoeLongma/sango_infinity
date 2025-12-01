using Sango.Game.Render;
using Sango.Game.Render.UI;
using Sango.Render;
using System.Collections.Generic;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;

namespace Sango.Game.Player
{
    public class TroopCommandAttack : TroopComandBase
    {
        protected List<Cell> MovePath { get; set; }
        protected List<Cell> spellRangeCell = new List<Cell>();
        protected Cell spellCell;
        protected Skill spellSkill;
        protected bool isShow = false;
        protected bool isMoving = false;
        public TroopCommandAttack()
        {
            customMenuName = "攻击";
            customMenuOrder = 0;
        }

        public override bool IsValid
        {
            get
            {
                bool hasTarget = false;
                // 攻击范围内必须有可攻击目标
                Cell stayCell = ActionCell;
                spellRangeCell.Clear();
                List<Cell> rangeCell = new List<Cell>();
                if (TargetTroop.NormalSkill != null)
                {
                    Skill skill = TargetTroop.NormalSkill.Skill;
                    if (skill.CanBeSpell(TargetTroop))
                    {
                        skill.GetSpellRange(TargetTroop, stayCell, rangeCell);
                        foreach (Cell c in rangeCell)
                        {
                            if (skill.CanSpeellToHere(TargetTroop, c))
                            {
                                spellRangeCell.Add(c);
                                hasTarget = true;
                            }
                        }
                    }   
                }
                rangeCell.Clear();
                if (TargetTroop.NormalRangeSkill != null)
                {
                    Skill skill = TargetTroop.NormalRangeSkill.Skill;
                    if (skill.CanBeSpell(TargetTroop))
                    {
                      
                        skill.GetSpellRange(TargetTroop, stayCell, rangeCell);
                        foreach(Cell c in rangeCell)
                        {
                            if (skill.CanSpeellToHere(TargetTroop, c))
                            {
                                spellRangeCell.Add(c);
                                hasTarget = true;
                            }
                        }
                    }   
                }

                return hasTarget;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            spellSkill = null;
            isShow = false;
            isMoving = false;
            ContextMenu.SetVisible(false);
            MovePath = Singleton<TroopSystem>.Instance.movePath;
            ShowSpellRange();
        }

        protected void ShowSpellRange()
        {
            if (spellRangeCell.Count == 0) return;
            MapRender mapRender = MapRender.Instance;
            for (int i = 0, count = spellRangeCell.Count; i < count; ++i)
            {
                Cell c = spellRangeCell[i];
                if (!MovePath.Contains(c))
                    mapRender.SetGridMaskColor(c.x, c.y, Color.red);
                mapRender.SetDarkMaskColor(c.x, c.y, Color.black);
            }
            mapRender.EndSetGridMask();
            mapRender.EndSetDarkMask();
            mapRender.SetDarkMask(true);
        }

        protected void ClearShowSpellRange()
        {
            if (spellRangeCell.Count == 0) return;
            MapRender mapRender = MapRender.Instance;
            for (int i = 0, count = spellRangeCell.Count; i < count; ++i)
            {
                Cell c = spellRangeCell[i];
                if (!MovePath.Contains(c))
                    mapRender.SetGridMaskColor(c.x, c.y, Color.black);
                mapRender.SetDarkMaskColor(c.x, c.y, Color.clear);

            }
            mapRender.EndSetGridMask();
            mapRender.EndSetDarkMask();
            mapRender.SetDarkMask(false);
        }

        public override void OnDestroy()
        {
            ClearShowSpellRange();
            spellRangeCell.Clear();
        }

        protected void OnMoveDone()
        {
            isMoving = false;
        }

        public override void Update()
        {
            if (isShow)
            {
                if (!isMoving)
                {
                    if (TargetTroop.SpellSkill(spellSkill, spellCell))
                    {
                        TargetTroop.ActionOver = true;
                        TargetTroop.Render?.UpdateRender();
                        Done();
                    }
                }
            }
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            if (isShow) return;

            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClick:
                    {
                        PlayerCommand.Instance.Back();
                        break;
                    }

                case CommandEventType.Click:
                    {
                        if (isOverUI) return;

                        if (spellRangeCell.Contains(cell))
                        {


                            Cell stayCell = MovePath[MovePath.Count - 1];
                            spellCell = cell;
                            if (spellSkill == null)
                            {
                                if (spellCell.Distance(stayCell) == 1)
                                    spellSkill = TargetTroop.NormalSkill.Skill;
                                else
                                    spellSkill = TargetTroop.NormalRangeSkill.Skill;
                            }

                            if (!spellSkill.CanSpeellToHere(TargetTroop, cell))
                                return;

                            Singleton<TroopActionMenu>.Instance.troopRender.Clear();
                            ContextMenu.CloseAll();
                            Cell start = TargetTroop.cell;

                            if(start  == stayCell)
                            {
                                isShow = true;
                                isMoving = false;
                                return;
                            }

                            for (int i = 1; i < MovePath.Count; i++)
                            {
                                bool isLast = i == MovePath.Count - 1;
                                Cell dest = MovePath[i];
                                TroopMoveEvent @event = new TroopMoveEvent()
                                {
                                    troop = TargetTroop,
                                    dest = dest,
                                    start = start,
                                    isLastMove = isLast
                                };

                                if (isLast)
                                {
                                    @event.doneAction = OnMoveDone;
                                }

                                RenderEvent.Instance.Add(@event);
                                start = dest;
                            }
                            isShow = true;
                            isMoving = true;
                        }
                        break;
                    }
            }
        }
    }
}
