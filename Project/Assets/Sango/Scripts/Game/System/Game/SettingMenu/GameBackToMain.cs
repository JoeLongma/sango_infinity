using Sango.Game.Render.UI;

namespace Sango.Game.Player
{
    [GameSystem]
    public class GameBackToMain : GameSettingMenuBase
    {
        public GameBackToMain()
        {
            customMenuName = "返回主菜单";
            customMenuOrder = 3;
        }

        public override void OnEnter()
        {
            UIDialog.Open("是否需要回到游戏主菜单", () =>
            {
                Done();
                UIDialog.Close();
                GameSystem.GetSystem<Player>().QuitToMainMenu();
            }).cancelAction = () =>
            {
                UIDialog.Close();
                Done();
            }; ;
        }

        public override void OnDestroy()
        {
            UIDialog.Close();
        }
    }
}
