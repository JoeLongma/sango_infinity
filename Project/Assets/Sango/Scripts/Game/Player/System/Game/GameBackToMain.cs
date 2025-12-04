using Sango.Game.Render.UI;

namespace Sango.Game.Player
{
    public class GameBackToMain : GameComandBase
    {
        public GameBackToMain() {
            customMenuName = "返回主页";
            customMenuOrder = 3;
        }

        public override void OnEnter()
        {
            UIDialog.Open("是否需要回到游戏主页", () =>
            {
                Done();
                UIDialog.Close();
                Player.Instance.QuitToMainMenu();
            });
        }
    }
}
