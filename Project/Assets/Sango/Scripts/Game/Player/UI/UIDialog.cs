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
    }
}
