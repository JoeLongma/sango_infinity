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
            commads.Push(command);
            command.OnEnter();
            CurrentCommand?.OnExit();
            CurrentCommand = command;
            GameController.Instance.Enabled = false;
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
                GameController.Instance.Enabled = true;
            }
        }

        public void Done()
        {
            CurrentCommand = null;
            while (commads.Count > 0)
                commads.Pop().OnDone();
            GameController.Instance.Enabled = true;
        }

        public void Update()
        {
            CurrentCommand?.Update();
        }

        public void HandleEvent(CommandEventType eventType, Cell clickCell, Vector3 clickPosition)
        {
            if (CurrentCommand != null)
            {
                CurrentCommand.HandleEvent(eventType, clickCell, clickPosition);
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
