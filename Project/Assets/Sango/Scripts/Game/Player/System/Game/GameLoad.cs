namespace Sango.Game.Player
{
    public class GameLoad : GameComandBase
    {
        public GameLoad()
        {
            customMenuName = "加载";
            customMenuOrder = 1;
            windowName = "window_scenario_save";
        }

        public override void OnEnter()
        {
            Window.WindowInterface windowInterface = Window.Instance.Open(windowName, false);
        }

        public override void OnDone()
        {
            Window.Instance.Close(windowName);
        }
    }
}
