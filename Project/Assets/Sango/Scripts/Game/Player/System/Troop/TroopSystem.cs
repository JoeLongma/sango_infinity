using Sango.Game.Render.UI;
using UnityEngine;
using ContextMenu = Sango.Game.Render.UI.ContextMenu;
namespace Sango.Game.Player
{
    public class TroopSystem : CommandSystemBase
    {
        public Troop TargetTroop { get; set; }
        public void Start(Troop troop, Vector3 startPoint)
        {
            ContextMenuData.MenuData.Clear();
            GameEvent.OnTroopContextMenuShow?.Invoke(ContextMenuData.MenuData, troop);
            if (!ContextMenuData.MenuData.IsEmpty())
            {
                TargetTroop = troop;
                ContextMenu.Show(ContextMenuData.MenuData, startPoint);
                PlayerCommand.Instance.Push(this);
            }
        }

        /// <summary>
        /// 进入当前命令的时候触发
        /// </summary>
        public override void OnEnter()
        {


        }

        /// <summary>
        /// 离开当前命令的时候触发
        /// </summary>
        public override void OnExit() {; }

        /// <summary>
        /// 当前命令被重新拾起的时候触发(返回)
        /// </summary>
        public override void OnBack() {; }

        /// <summary>
        /// 当前命令被舍弃的时候触发
        /// </summary>
        public override void OnDestroy() {; }

        /// <summary>
        /// 结束整个命令链的时候触发
        /// </summary>
        public override void OnDone() {; }

        /// <summary>
        /// 每帧更新
        /// </summary>
        public override void Update() {; }

        public override void HandleEvent(CommandEventType eventType, Cell cell) {; }
    }
}
