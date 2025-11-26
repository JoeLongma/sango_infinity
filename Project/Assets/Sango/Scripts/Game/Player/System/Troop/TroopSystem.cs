using Sango.Game.Render.UI;
using Sango.Render;
using System.Collections.Generic;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;
namespace Sango.Game.Player
{
    public class TroopSystem : CommandSystemBase
    {
        public override void Init()
        {
            base.Init();
            Singleton<TroopCommandStay>.Instance.Init();
            Singleton<TroopCommandSkill>.Instance.Init();
            Singleton<TroopCommandAttack>.Instance.Init();
        }

        public List<Cell> moveRange = new List<Cell>();
        public List<Cell> movePath = new List<Cell>();

        public Troop TargetTroop { get; set; }

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
                PlayerCommand.Instance.Push(this);
            }
            else
            {
                Singleton<TroopMenu>.Instance.Start(troop, startPoint); 
            }
        }

        public override void OnEnter()
        {
            moveRange.Clear();
            Scenario.Cur.Map.GetMoveRange(TargetTroop, moveRange);
            OnBack();
        }

        public override void OnExit()
        {
            GameController.Instance.RotateViewEnabled = false;
            GameController.Instance.ZoomViewEnabled = false;
            ClearShowMoveRange();
            ShowMovePath();
        }

        public override void OnBack()
        {
            GameController.Instance.RotateViewEnabled = true;
            GameController.Instance.ZoomViewEnabled = true;
            ShowMoveRange();
        }

        /// <summary>
        /// 离开当前命令的时候触发
        /// </summary>
        public override void OnDestroy()
        {
            // 这个一定先清理
            ClearShowMovePath();
            ClearShowMoveRange();
        }

        public void ShowMoveRange()
        {
            MapRender mapRender = MapRender.Instance;
            for (int i = 0, count = moveRange.Count; i < count; ++i)
            {
                Cell cell = moveRange[i];
                mapRender.SetGridMaskColor(cell.x, cell.y, Color.green);
            }
            mapRender.EndSetGridMask();
        }

        public void ClearShowMoveRange()
        {
            if (moveRange.Count == 0) return;
            MapRender mapRender = MapRender.Instance;
            for (int i = 0, count = moveRange.Count; i < count; ++i)
            {
                Cell cell = moveRange[i];
                mapRender.SetGridMaskColor(cell.x, cell.y, Color.black);
            }
            mapRender.EndSetGridMask();
        }
        public void ShowMovePath()
        {
            MapRender mapRender = MapRender.Instance;
            for (int i = 0, count = movePath.Count; i < count; ++i)
            {
                Cell c = movePath[i];
                mapRender.SetGridMaskColor(c.x, c.y, Color.blue);
            }
            mapRender.EndSetGridMask();
        }

        public void ClearShowMovePath()
        {
            if (movePath.Count == 0) return;
            MapRender mapRender = MapRender.Instance;
            for (int i = 0, count = movePath.Count; i < count; ++i)
            {
                Cell c = movePath[i];
                mapRender.SetGridMaskColor(c.x, c.y, Color.green);
            }
            mapRender.EndSetGridMask();
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
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

                        if (moveRange.Contains(cell))
                        {
                            movePath.Clear();
                            Scenario.Cur.Map.GetMovePath(TargetTroop, cell, movePath);
                            Singleton<TroopActionMenu>.Instance.Start(TargetTroop, cell, clickPosition);
                        }
                        break;
                    }
            }
        }
    }
}
