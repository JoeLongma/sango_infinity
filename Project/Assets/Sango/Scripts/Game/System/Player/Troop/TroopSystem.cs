using Sango.Render;
using System.Collections.Generic;
using UnityEngine;
namespace Sango.Game.Player
{
    public class TroopSystem : GameSystem
    {
        public override void Init()
        {
            GameEvent.OnClick += OnClick;
            Singleton<TroopActionStay>.Instance.Init();
            Singleton<TroopActionSkill>.Instance.Init();
            Singleton<TroopActionAttack>.Instance.Init();
            Singleton<TroopActionBuild>.Instance.Init();
            Singleton<TroopActionBuildingFix>.Instance.Init();

            Singleton<TroopActionStrategy>.Instance.Init();

            Singleton<TroopInteractiveBuildingFix>.Instance.Init();
            Singleton<TroopInteractiveCityEnter>.Instance.Init();
        }

        void OnClick(Cell clickCell, Vector3 clickPosition, bool isOverUI)
        {
            if (clickCell.troop == null) return;

            Troop troop = clickCell.troop;
            if (!troop.IsAlive) return;
            TargetTroop = troop;
            if (!troop.ActionOver && troop.BelongForce.IsPlayer && troop.BelongForce == Scenario.Cur.CurRunForce)
            {
                Enter();
            }
            else
            {
                Singleton<TroopMenu>.Instance.Start(troop, clickPosition);
            }
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

        }

        public override void OnEnter()
        {
            TargetTroop.Render?.SetFlash(true);

            moveRange.Clear();
            Scenario.Cur.Map.GetMoveRange(TargetTroop, moveRange);
            OnBack(null);

        }

        public override void OnExit()
        {
            GameController.Instance.RotateViewEnabled = false;
            GameController.Instance.ZoomViewEnabled = false;
            GameController.Instance.KeyboardMoveEnabled = false;
            GameController.Instance.DragMoveViewEnabled = false;
            GameController.Instance.BorderMoveViewEnabled = false;
            ClearShowMoveRange();
            ShowMovePath();
        }

        public override void OnBack(ICommandEvent whoGone)
        {
            GameController.Instance.RotateViewEnabled = true;
            GameController.Instance.ZoomViewEnabled = true;
            GameController.Instance.KeyboardMoveEnabled = true;
            GameController.Instance.DragMoveViewEnabled = true;
            GameController.Instance.BorderMoveViewEnabled = true;
            ShowMoveRange();
        }

        /// <summary>
        /// 离开当前命令的时候触发
        /// </summary>
        public override void OnDestroy()
        {
            TargetTroop.Render?.SetFlash(false);

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
                        GameSystemManager.Instance.Back();
                        break;
                    }

                case CommandEventType.Click:
                    {
                        if (isOverUI) return;

                        if (!moveRange.Contains(cell))
                            return;

                        // 排除出征城市,出征城市无法原地进行行为
                        if ((cell == TargetTroop.cell && cell.building == null) || cell.IsEmpty())
                        {
                            movePath.Clear();
                            Scenario.Cur.Map.GetMovePath(TargetTroop, cell, movePath);
                            Singleton<TroopActionMenu>.Instance.Start(TargetTroop, cell, clickPosition);
                            return;
                        }

                        // 排除出征城市
                        if (TargetTroop.cell.building == TargetTroop.BelongCity && cell.building == TargetTroop.BelongCity)
                            return;

                        movePath.Clear();
                        Scenario.Cur.Map.GetMovePath(TargetTroop, cell, movePath);
                        // 进入
                        Singleton<TroopInteractiveDialog>.Instance.Start(TargetTroop, cell, clickPosition);


                        //if (cell.building != null)
                        //{
                        //    if(cell.building.IsCityBase())
                        //    {
                        //        if(cell.building.IsSameForce(TargetTroop))
                        //        {
                        //                 }
                        //        else
                        //        {
                        //            // 委任攻击
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (cell.building.IsSameForce(TargetTroop) && !cell.building.isComplte)
                        //        {
                        //            // 修建

                        //            return;
                        //        }
                        //        else if(cell.building.IsEnemy(TargetTroop))
                        //        {
                        //            // 委任攻击
                        //        }
                        //        else
                        //        {
                        //            // 接近
                        //        }
                        //    }
                        //    return;
                        //}

                        //if (cell.troop != null)
                        //{
                        //    if (cell.building.IsEnemy(TargetTroop))
                        //    {
                        //        // 歼灭 或者 驱逐
                        //    }
                        //    else
                        //    {
                        //        // 接近
                        //    }
                        //}

                        break;
                    }
            }
        }
    }
}
