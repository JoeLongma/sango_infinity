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
        public enum DialogStyle
        {
            Normal,
            ChoosePersonSay,
            Window,
            ClickPersonSay,
        }

        public Text content;
        public System.Action cancelAction;
        public System.Action sureAction;
        public RectTransform panelRect;
        public RectTransform btnRect;
        public RawImage headImg;
        public Text nameText;

        static UIDialog CurInstance;

        public void OnSure()
        {
            sureAction?.Invoke();
        }

        public void OnCancel()
        {
            cancelAction?.Invoke();
        }

        public static UIDialog Open(string content, System.Action sureAction)
        {
            return Open(content, sureAction, Input.mousePosition);
        }

        public static UIDialog Open(DialogStyle dialogStyle, string content, System.Action sureAction)
        {
            string windowName = "window_dialog";
            switch (dialogStyle)
            {
                case DialogStyle.ChoosePersonSay:
                    windowName = "window_dialog2"; break;
                case DialogStyle.Window:
                    windowName = "window_dialog3"; break;
                case DialogStyle.ClickPersonSay:
                    windowName = "window_dialog4"; break;
            }
            return Open(windowName, content, sureAction, Input.mousePosition);
        }

        public static UIDialog Open(string content, System.Action sureAction, Vector3 startPoint)
        {
            return Open("window_dialog", content, sureAction, startPoint);
        }

        public static UIDialog Open(string windowName, string content, System.Action sureAction, Vector3 startPoint)
        {
            Window.WindowInterface windowInterface = Window.Instance.Open(windowName);
            if (windowInterface.ugui_instance == null)
                return null;

            UIDialog uIDialog = windowInterface.ugui_instance.GetComponent<UIDialog>();
            if (uIDialog == null) return null;

            uIDialog.content.text = content;
            uIDialog.sureAction = sureAction;
            uIDialog.cancelAction = Close;
            CurInstance = uIDialog;

            if (uIDialog.btnRect != null)
            {
                Vector2 anchorPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(uIDialog.GetComponent<RectTransform>(),
                    startPoint, Game.Instance.UICamera, out anchorPos);

                uIDialog.btnRect.anchoredPosition = anchorPos + new Vector2(-74, 0);
            }

            return uIDialog;
        }


        public void SetPerson(Person person)
        {
            if (headImg == null || nameText == null) return;
            headImg.texture = GameRenderHelper.LoadHeadIcon(person.headIconID, 1);
            nameText.text = person.Name;
        }

        public static void Close()
        {
            CurInstance?.Hide();
            CurInstance = null;
        }
    }
}
