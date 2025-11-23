using Sango.Game.Render.UI;
using Sango.Render;
using System.Collections.Generic;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;

namespace Sango.Game.Player
{
    public class TroopMove : CommandSystemBase
    {
        public Troop TargetTroop { get; set; }

        List<Cell> moveRange = new List<Cell>();
        List<Cell> movePath = new List<Cell>();
        enum ControlType
        {
            SelectDest,
            Move
        }
        ControlType CurrentControlType { get; set; }

        public virtual void Start(Troop troop)
        {
            TargetTroop = troop;
            PlayerCommand.Instance.Push(this);
        }

        public override void OnEnter()
        {
            CurrentControlType = ControlType.SelectDest;
            moveRange.Clear();
            movePath.Clear();
            base.OnEnter();
            Scenario.Cur.Map.GetMoveRange(TargetTroop, moveRange);

            MapRender mapRender = MapRender.Instance;
            for (int i = 0, count = moveRange.Count; i < count; ++i)
            {
                Cell cell = moveRange[i];
                mapRender.SetGridMaskColor(cell.x, cell.y, Color.green);
            }
            mapRender.EndSetGridMask();

        }

        public override void OnDestroy()
        {
            MapRender mapRender = MapRender.Instance;
            for (int i = 0, count = moveRange.Count; i < count; ++i)
            {
                Cell cell = moveRange[i];
                mapRender.SetGridMaskColor(cell.x, cell.y, Color.black);
            }
            mapRender.EndSetGridMask();
        }

        public override void Update()
        {
            base.Update();
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
                            case ControlType.SelectDest:
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
