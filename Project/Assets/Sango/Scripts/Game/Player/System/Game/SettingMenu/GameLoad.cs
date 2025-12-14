namespace Sango.Game.Player
{
    public class GameLoad : GameSettingMenuBase
    {
        public GameLoad()
        {
            customMenuName = "加载";
            customMenuOrder = 1;
            windowName = "window_scenario_save_in_game";
        }

        public override void OnEnter()
        {
            Window.WindowInterface windowInterface = Window.Instance.Open(windowName, false);
        }

        public override void OnDestroy()
        {
            Window.Instance.Close(windowName);
        }

    }
}
