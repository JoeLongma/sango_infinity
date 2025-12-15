using Sango.Game.Player;

namespace Sango.Game.Render.UI
{
    /// <summary>
    /// 游戏开始界面
    /// </summary>
    public class UIMobileCancel : UGUIWindow
    {
        public void OnCancel()
        {
            GameController.Instance.OnCancel();
        }
    }
}
