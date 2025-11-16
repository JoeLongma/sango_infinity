using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Sango.Game.Player
{

    public class PlayerCommand : Singletion<PlayerCommand>
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
            CurrentCommand.OnDone();
            CurrentCommand = null;
            commads.Clear();
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
                CurrentCommand.HandleEvent(eventType, clickCell);
                return;
            }

            switch (eventType)
            {
                case CommandEventType.Click:
                    {
                        if (clickCell.building != null)
                        {
                            BuildingSystem.Instance.Start(clickCell.building, clickPosition);
                        }
                        else if (clickCell.troop != null)
                        {
                            TroopSystem.Instance.Start(clickCell.troop, clickPosition);
                        }
                    }
                    break;
                case CommandEventType.Cancel:
                    break;
            }
        }

    }

}
