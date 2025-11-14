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
            CurrentCommand = commads.Peek();
            CurrentCommand.OnBack();
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
            if(CurrentCommand != null)
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
                case CommandEventType.RClick:
                    break;
                case CommandEventType.Cancel:
                    break;
            }

            //StringBuilder stringBuilder = new StringBuilder();

            //stringBuilder.Append("eventType =");
            //stringBuilder.Append(eventType.ToString());
            //stringBuilder.Append(" x =");
            //stringBuilder.Append(clickCell.x);
            //stringBuilder.Append(" y =");
            //stringBuilder.Append(clickCell.y);
            //stringBuilder.Append(" 地形:");
            //stringBuilder.Append(clickCell.TerrainType.Name);
            //stringBuilder.Append(" ");
            //if (!clickCell.IsEmpty())
            //{
            //    if (clickCell.troop != null)
            //        stringBuilder.Append(clickCell.troop.Name);
            //    else
            //        stringBuilder.Append(clickCell.building.Name);
            //}

            //Sango.Log.Error(stringBuilder);
        }
    }

}
