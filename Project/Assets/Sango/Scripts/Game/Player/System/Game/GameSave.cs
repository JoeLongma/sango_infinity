namespace Sango.Game.Player
{
    public class GameSave : GameComandBase
    {
        public GameSave() {
            customMenuName = "保存";
            customMenuOrder = 0;
            windowName = "window_scenario_save";
        }

        public override void OnEnter()
        {
            Window.WindowInterface windowInterface = Window.Instance.Open(windowName, true);
        }

        public override void OnDone()
        {
            Window.Instance.Close(windowName);
        }
    }
}
