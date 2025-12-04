namespace Sango.Game.Render.UI
{
    /// <summary>
    /// 游戏开始界面
    /// </summary>
    public class UIStart : UGUIWindow
    {
        public void OnNewGame()
        {
            Game.Instance.StartNewGame();
        }

        public void OnMapEditor()
        {
            Game.Instance.EnterMapEditor();
        }

        public void OnLoadGame()
        {
            Window.Instance.Open("window_scenario_save", false);
        }

        public void OnTest()
        {
            string path = Sango.Path.FindFile("Scenario/Scenario.json");
            Scenario scenario = new Scenario(path);
            Scenario.StartScenario(scenario);
        }
    }
}
