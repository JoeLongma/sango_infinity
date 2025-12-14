namespace Sango.Game.Player
{
    public class GameSave : GameSettingMenuBase
    {
        public GameSave() {
            customMenuName = "保存";
            customMenuOrder = 0;
            windowName = "window_scenario_save_in_game";
        }

        public override void OnEnter()
        {
            Window.WindowInterface windowInterface = Window.Instance.Open(windowName, true);
        }

        public override void OnDestroy()
        {
            Window.Instance.Close(windowName);
        }
    }
}
