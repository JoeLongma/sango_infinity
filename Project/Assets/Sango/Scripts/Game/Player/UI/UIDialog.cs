using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    /// <summary>
    /// 游戏开始界面
    /// </summary>
    public class UIDialog : UGUIWindow
    {
        public Text content;
        public Action cancelAction;
        public Action sureAction;
        public RectTransform panelRect;
        public RectTransform btnRect;

        public void OnSure()
        {
            sureAction?.Invoke();
        }

        public void OnCancel()
        {
            cancelAction?.Invoke();
        }

        public static UIDialog Open(string content, Action sureAction)
        {
            Window.WindowInterface windowInterface = Window.Instance.Open("window_dialog");
            if (windowInterface.ugui_instance == null)
                return null;

            UIDialog uIDialog = windowInterface.ugui_instance.GetComponent<UIDialog>();
            if (uIDialog == null) return null;

            uIDialog.content.text = content;
            uIDialog.sureAction = sureAction;
            uIDialog.cancelAction = Close;
            return uIDialog;
        }

        public static void Close()
        {
            Window.Instance.Close("window_dialog");
        }
    }
}
