using Sango.Game.Render.UI;
using UnityEngine;

namespace Sango.Game.Player
{
    /// <summary>
    /// 游戏右键菜单
    /// </summary>
    [GameSystem(auto = true)]
    public class GameRightButtonMenu : GameSystem
    {
        public override void Init()
        {
            GameEvent.OnRClick += OnRClick;
            GameEvent.OnRClickObject += OnRClickObject;
        }

        void OnRClick(Cell clickCell, Vector3 clickPosition, bool isOverUI)
        {
            if (Scenario.Cur.CurRunForce.IsPlayer)
            {
                ContextMenuData.MenuData.Clear();
                GameEvent.OnRightMouseButtonContextMenuShow?.Invoke(ContextMenuData.MenuData);
                if (!ContextMenuData.MenuData.IsEmpty())
                {
                    // 特殊处理<进行>
                    UIContextMenu contextMenu = Render.UI.ContextMenu.Show(ContextMenuData.MenuData, clickPosition, ContextMenuType.Command);
                    PlayerEndTurn playerEndTurn = GameSystem.GetSystem<PlayerEndTurn>();
                    if (playerEndTurn != null)
                    {
                        if (contextMenu != null && contextMenu.menuItem.Length > 1)
                        {
                            UIMenuItem item = contextMenu.menuItem[1];
                            if (item != null)
                            {
                                item.SetTitle(playerEndTurn.customTitleName).SetListener(() =>
                                {
                                    Render.UI.ContextMenu.CloseAll();
                                    playerEndTurn.Push();
                                });
                            }
                        }
                    }
                    Enter();
                }
            }
        }

        void OnRClickObject(Cell clickCell, Vector3 clickPosition, bool isOverUI)
        {
            if (Scenario.Cur.CurRunForce.IsPlayer)
            {
                ContextMenuData.MenuData.Clear();
                ContextMenuType menuType = ContextMenuType.System;
                if (clickCell.building != null)
                {
                    if(clickCell.building.IsPort())
                    {
                        GameEvent.OnPortRightMouseButtonContextMenuShow?.Invoke(ContextMenuData.MenuData, clickCell.building as Port);
                    }
                    else if (clickCell.building.IsGate())
                    {
                        GameEvent.OnGateRightMouseButtonContextMenuShow?.Invoke(ContextMenuData.MenuData, clickCell.building as Gate);
                    }
                    else if (clickCell.building.IsCity())
                    {
                        GameEvent.OnCityRightMouseButtonContextMenuShow?.Invoke(ContextMenuData.MenuData, clickCell.building as City);
                    }
                    else
                    {
                        GameEvent.OnBuildingRightMouseButtonContextMenuShow?.Invoke(ContextMenuData.MenuData, clickCell.building as Building);
                    }
                }
                else if(clickCell.troop != null)
                {
                    GameEvent.OnTroopRightMouseButtonContextMenuShow?.Invoke(ContextMenuData.MenuData, clickCell.troop);
                }
                

                if (!ContextMenuData.MenuData.IsEmpty())
                {
                    // 特殊处理<进行>
                    UIContextMenu contextMenu = Render.UI.ContextMenu.Show(ContextMenuData.MenuData, clickPosition, menuType);
                    PlayerEndTurn playerEndTurn = GameSystem.GetSystem<PlayerEndTurn>();
                    if (playerEndTurn != null)
                    {
                        if (contextMenu != null && contextMenu.menuItem.Length > 1)
                        {
                            UIMenuItem item = contextMenu.menuItem[1];
                            if (item != null)
                            {
                                item.SetTitle(playerEndTurn.customTitleName).SetListener(() =>
                                {
                                    Render.UI.ContextMenu.CloseAll();
                                    playerEndTurn.Push();
                                });
                            }
                        }
                    }
                    Enter();
                }
            }
        }


        public override void OnDestroy()
        {
            Render.UI.ContextMenu.CloseAll();
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClickDown:
                    Exit(); break;

                case CommandEventType.ClickDown:
                    {
                        if (!isOverUI)
                            Exit();
                        break;
                    }
            }

        }
    }
}
