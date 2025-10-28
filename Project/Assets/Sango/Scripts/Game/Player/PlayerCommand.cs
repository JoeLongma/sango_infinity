using System.Collections.Generic;

namespace Sango.Game.Player
{


    public class PlayerCommand : Singletion<PlayerCommand>
    {
        Stack<CommandBase> commads = new Stack<CommandBase>();
        public CommandBase CurrentCommand { get; private set; }

        public void Init()
        {

        }

        public void Push(CommandBase command)
        {
            commads.Push(command);
            command.OnEnter();
            CurrentCommand?.OnExit();
            CurrentCommand = command;
            GameController.Instance.Enabled = false;
        }

        public void Back()
        {
            CommandBase command = commads.Pop();
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
    }

}
