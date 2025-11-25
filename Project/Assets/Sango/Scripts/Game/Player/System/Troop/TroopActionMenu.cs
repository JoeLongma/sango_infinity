using Sango.Game.Render;
using Sango.Game.Render.UI;
using Sango.Render;
using System.Collections.Generic;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;
namespace Sango.Game.Player
{
    public class TroopActionMenu : CommandSystemBase
    {
        public List<Cell> spellRangeCell = new List<Cell>();
        public Troop TargetTroop { get; set; }
        public Cell TargetCell { get; set; }

        TroopRender troopRender;

        public void Start(Troop troop, Cell targetCell, Vector3 startPoint)
        {
            TargetTroop = troop;
            TargetCell = targetCell;
            ContextMenuData.MenuData.Clear();
            GameEvent.OnTroopActionContextMenuShow?.Invoke(ContextMenuData.MenuData, troop, targetCell);
            if (!ContextMenuData.MenuData.IsEmpty())
            {
                ContextMenu.Show(ContextMenuData.MenuData, startPoint);
                PlayerCommand.Instance.Push(this);
            }
        }

        public override void OnEnter()
        {
            troopRender = new TroopRender(TargetTroop, false);
            troopRender.SetPosition(TargetCell.Position);
            List<SkillInstance> list;
            if (TargetCell.TerrainType.isWater)
                list = TargetTroop.waterSkills;
            else
                list = TargetTroop.landSkills;
            for (int i = 0, count = list.Count; i < count; ++i)
            {
                Skill skill = list[i].Skill;
                if (skill.CanBeSpell(TargetTroop))
                    skill.GetSpellRange(TargetTroop, TargetCell, spellRangeCell);
            }
            ShowSpellRange();
        }

        /// <summary>
        /// 离开当前命令的时候触发
        /// </summary>
        public override void OnDestroy()
        {
            troopRender.Clear();
            troopRender = null;
            ClearShowSpellRange();
            ContextMenu.CloseAll();
        }

        public override void OnExit()
        {
            ClearShowSpellRange();
            ContextMenu.SetVisible(false);
        }

        public override void OnBack()
        {
            ShowSpellRange();
            ContextMenu.SetVisible(true);
        }

        public void ShowSpellRange()
        {
            MapRender mapRender = MapRender.Instance;
            for (int i = 0, count = spellRangeCell.Count; i < count; ++i)
            {
                Cell c = spellRangeCell[i];
                if(c.moveAble)
                    mapRender.SetGridMaskColor(c.x, c.y, Color.red);
            }
            mapRender.EndSetGridMask();
            mapRender.SetDarkMask(true);
        }

        public void ClearShowSpellRange()
        {
            if (spellRangeCell.Count == 0) return;
            MapRender mapRender = MapRender.Instance;
            for (int i = 0, count = spellRangeCell.Count; i < count; ++i)
            {
                Cell c = spellRangeCell[i];
                mapRender.SetGridMaskColor(c.x, c.y, Color.black);
            }
            mapRender.EndSetGridMask();
            mapRender.SetDarkMask(false);
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClick:
                    {
                        ContextMenu.CloseAll();
                        PlayerCommand.Instance.Back();
                        break;
                    }

                case CommandEventType.Click:
                    {
                        break;
                    }
            }
        }
    }
}
