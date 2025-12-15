using Sango.Game.Render.UI;

namespace Sango.Game.Player
{
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
                Player.Instance.QuitToMainMenu();
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
