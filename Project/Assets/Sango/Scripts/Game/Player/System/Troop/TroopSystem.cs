using Sango.Game.Render.UI;
using Sango.Render;
using System.Collections.Generic;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;
namespace Sango.Game.Player
{
    public class TroopSystem : CommandSystemBase
    {
        public List<Cell> moveRange = new List<Cell>();
        public List<Cell> movePath = new List<Cell>();
        public Troop TargetTroop { get; set; }

        enum ControlType
        {
            OtherMenu,
            Move
        }
        ControlType CurrentControlType { get; set; }

        public void Start(Troop troop)
        {
            Start(troop, Vector3.zero);
        }

        public void Start(Troop troop, Vector3 startPoint)
        {
            if (!troop.IsAlive) return;
            TargetTroop = troop;
            if (!troop.ActionOver && troop.BelongForce.IsPlayer && troop.BelongForce == Scenario.Cur.CurRunForce)
            {
                CurrentControlType = ControlType.Move;
                moveRange.Clear();
                movePath.Clear();
                Scenario.Cur.Map.GetMoveRange(TargetTroop, moveRange);
                MapRender mapRender = MapRender.Instance;
                for (int i = 0, count = moveRange.Count; i < count; ++i)
                {
                    Cell cell = moveRange[i];
                    mapRender.SetGridMaskColor(cell.x, cell.y, Color.green);
                }
                mapRender.EndSetGridMask();
                PlayerCommand.Instance.Push(this);
            }
            else
            {
                ContextMenuData.MenuData.Clear();
                GameEvent.OnTroopContextMenuShow?.Invoke(ContextMenuData.MenuData, troop);
                if (!ContextMenuData.MenuData.IsEmpty())
                {
                    TargetTroop = troop;
                    ContextMenu.Show(ContextMenuData.MenuData, startPoint);
                    CurrentControlType = ControlType.OtherMenu;
                    PlayerCommand.Instance.Push(this);
                }
            }
        }

        /// <summary>
        /// 离开当前命令的时候触发
        /// </summary>
        public override void OnDestroy()
        {
            MapRender mapRender = MapRender.Instance;
            for (int i = 0, count = moveRange.Count; i < count; ++i)
            {
                Cell cell = moveRange[i];
                mapRender.SetGridMaskColor(cell.x, cell.y, Color.black);
            }
            mapRender.EndSetGridMask();

            ContextMenu.CloseAll();
        }


        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition)
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
                        switch (CurrentControlType)
                        {
                            case ControlType.Move:
                                {
                                    if (moveRange.Contains(cell))
                                    {
                                        MapRender mapRender = MapRender.Instance;
                                        for (int i = 0, count = movePath.Count; i < count; ++i)
                                        {
                                            Cell c = movePath[i];
                                            mapRender.SetGridMaskColor(c.x, c.y, Color.green);
                                        }
                                        movePath.Clear();
                                        Scenario.Cur.Map.GetMovePath(TargetTroop, cell, movePath);
                                        for (int i = 0, count = movePath.Count; i < count; ++i)
                                        {
                                            Cell c = movePath[i];
                                            mapRender.SetGridMaskColor(c.x, c.y, Color.blue);
                                        }
                                        mapRender.EndSetGridMask();

                                        ContextMenu.CloseAll();

                                        // 显示确认菜单
                                        ContextMenuData.MenuData.Clear();
                                        GameEvent.OnTroopContextMenuShow?.Invoke(ContextMenuData.MenuData, TargetTroop);
                                        if (!ContextMenuData.MenuData.IsEmpty())
                                        {
                                            ContextMenu.Show(ContextMenuData.MenuData, clickPosition);
                                        }
                                    }
                                }
                                break;
                        }
                        break;
                    }
            }
        }
    }
}
