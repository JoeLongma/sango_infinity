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

            scenario.Info.cameraPosition = new UnityEngine.Vector3(1407, 0, 796);
            scenario.Info.cameraRotation = new UnityEngine.Vector3(40f, -50f, 0f);
            scenario.Info.cameraDistance = 400f;

            Scenario.StartScenario(scenario);
        }
    }
}
