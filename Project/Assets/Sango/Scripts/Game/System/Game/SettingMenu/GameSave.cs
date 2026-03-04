namespace Sango.Game.Player
{
    [GameSystem(auto = true)]
    public class GameSave : GameSettingMenuBase
    {
        public GameSave() {
            customMenuName = "保存";
            customMenuOrder = 0;
            windowName = "window_scenario_save_in_game";
        }

        public override void OnEnter()
        {
            Window.WindowInterface windowInterface = Window.Instance.Open(windowName, 0);
        }

        public override void OnDestroy()
        {
            Window.Instance.Close(windowName);
        }
    }
}
