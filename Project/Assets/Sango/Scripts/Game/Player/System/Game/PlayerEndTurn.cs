using Sango.Game.Render.UI;

namespace Sango.Game.Player
{
    public class PlayerEndTurn : CommandSystemBase
    {
        public PlayerEndTurn() {
            
        }

        public override void Init()
        {
            GameEvent.OnRightMouseButtonContextMenuShow += OnRightMouseButtonContextMenuShow;
        }

        public override void Clear()
        {
            GameEvent.OnRightMouseButtonContextMenuShow -= OnRightMouseButtonContextMenuShow;
        }

        protected virtual void OnRightMouseButtonContextMenuShow(ContextMenuData menuData)
        {
            menuData.Add("结束回合", 0, null, OnClickMenuItem, true);
        }

        protected virtual void OnClickMenuItem(ContextMenuItem contextMenuItem)
        {
            ContextMenu.CloseAll();
            PlayerCommand.Instance.Push(this);
        }

        public override void OnEnter()
        {
            UIDialog.Open("是否需要结束玩家回合", () =>
            {
                Scenario.Cur.CurRunForce.CurRunCorps.ActionOver = true;
                UIDialog.Close();
                Done();
            }).cancelAction = ()=>
            {
                UIDialog.Close();
                Done();
            };
        }

        public override void OnDestroy()
        {
            UIDialog.Close();
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClickUp:
                    PlayerCommand.Instance.Back(); break;
            }

        }
    }
}
