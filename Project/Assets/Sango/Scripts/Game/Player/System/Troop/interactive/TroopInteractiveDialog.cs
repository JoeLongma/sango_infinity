using Sango.Game.Render;
using Sango.Game.Render.UI;
using System.Collections.Generic;
using UnityEngine;
namespace Sango.Game.Player
{
    public class TroopInteractiveDialog : CommandSystemBase
    {
        public List<Cell> MovePath;
        public Troop TargetTroop { get; set; }
        public Cell TargetCell { get; set; }

        public TroopRender troopRender;

        public void Start(Troop troop, Cell targetCell, Vector3 startPoint)
        {
            TargetTroop = troop;
            TargetCell = targetCell;
            TroopInteractiveDialogData.InteractiveDialogData.Clear();
            GameEvent.OnTroopInteractiveContextDialogShow?.Invoke(TroopInteractiveDialogData.InteractiveDialogData, troop, targetCell);
            if (!TroopInteractiveDialogData.InteractiveDialogData.IsEmpty())
            {
                UIDialog dialog = UIDialog.OpenPersonSay(TroopInteractiveDialogData.InteractiveDialogData.content,
                    TroopInteractiveDialogData.InteractiveDialogData.sureAction);
                dialog.cancelAction = () =>
                {
                    UIDialog.Close();
                    PlayerCommand.Instance.Back();
                };
                dialog.SetPerson(troop.Leader);
                PlayerCommand.Instance.Push(this);
            }
        }

        public override void OnEnter()
        {
            MovePath = Singleton<TroopSystem>.Instance.movePath;
            Singleton<TroopSystem>.Instance.ShowMoveRange();
            Singleton<TroopSystem>.Instance.ShowMovePath();
        }

        /// <summary>
        /// 离开当前命令的时候触发
        /// </summary>
        public override void OnExit()
        {
            UIDialog.Close();
        }

        public override void OnDestroy()
        {
            UIDialog.Close();
            Singleton<TroopSystem>.Instance.ClearShowMovePath();
            Singleton<TroopSystem>.Instance.ClearShowMoveRange();
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.RClickUp:
                    {
                        PlayerCommand.Instance.Back();
                        break;
                    }
            }
        }
    }
}
