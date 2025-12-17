using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Sango.Game.Player
{

    public class PlayerCommand : Singleton<PlayerCommand>
    {
        Stack<ICommandEvent> commads = new Stack<ICommandEvent>();
        public ICommandEvent CurrentCommand { get; private set; }


        public void Push(ICommandEvent command)
        {
            if(CurrentCommand == null)
            {
                GameController.Instance.KeyboardMoveEnabled = false;
                GameController.Instance.RotateViewEnabled = false;
                GameController.Instance.DragMoveViewEnabled = false;
                GameController.Instance.ZoomViewEnabled = false;
                GameController.Instance.BorderMoveViewEnabled = false;
#if UNITY_ANDROID && !UNITY_EDITOR
                Window.Instance.Open("window_mobile_cancel");
#endif
            }

            commads.Push(command);
            CurrentCommand?.OnExit();
            CurrentCommand = command;
            command.OnEnter();
        }

        public void Back()
        {
            ICommandEvent command = commads.Pop();
            command.OnDestroy();
            if (commads.Count > 0)
            {
                CurrentCommand = commads.Peek();
                CurrentCommand.OnBack();
            }
            else
            {
                CurrentCommand = null;
                GameController.Instance.KeyboardMoveEnabled = true;
                GameController.Instance.RotateViewEnabled = true;
                GameController.Instance.DragMoveViewEnabled = true;
                GameController.Instance.ZoomViewEnabled = true;
                GameController.Instance.BorderMoveViewEnabled = true;
#if UNITY_ANDROID && !UNITY_EDITOR
                Window.Instance.Close("window_mobile_cancel");
#endif
            }
        }

        public void Done()
        {
            CurrentCommand = null;
            while (commads.Count > 0)
                commads.Pop().OnDone();
            GameController.Instance.KeyboardMoveEnabled = true;
            GameController.Instance.RotateViewEnabled = true;
            GameController.Instance.DragMoveViewEnabled = true;
            GameController.Instance.ZoomViewEnabled = true;
            GameController.Instance.BorderMoveViewEnabled = true;
#if UNITY_ANDROID && !UNITY_EDITOR
            Window.Instance.Close("window_mobile_cancel");
#endif
        }

        public void Update()
        {
            CurrentCommand?.Update();
        }

        public void HandleEvent(CommandEventType eventType, Cell clickCell, Vector3 clickPosition, bool isOverUI)
        {
            if (CurrentCommand != null)
            {
                CurrentCommand.HandleEvent(eventType, clickCell, clickPosition, isOverUI);
                return;
            }

            switch (eventType)
            {
                case CommandEventType.Click:
                    {
                        if (clickCell.building != null)
                        {
                            Singleton<BuildingSystem>.Instance.Start(clickCell.building, clickPosition);
                        }
                        else if (clickCell.troop != null)
                        {
                            Singleton<TroopSystem>.Instance.Start(clickCell.troop, clickPosition);
                        }
                    }
                    break;
                case CommandEventType.Cancel:
                    break;
            }
        }

    }

}
