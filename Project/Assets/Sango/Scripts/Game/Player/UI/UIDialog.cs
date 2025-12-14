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
        public Image headImg;
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

        public static UIDialog Open(string content, Action sureAction)
        {
            return Open(content, sureAction, Input.mousePosition);
        }

        public static UIDialog OpenPersonSay(string content, Action sureAction)
        {
            return Open("window_dialog2", content, sureAction, Input.mousePosition);
        }

        public static UIDialog Open(string content, Action sureAction, Vector3 startPoint)
        {
            return Open("window_dialog", content, sureAction, startPoint);
        }

        public static UIDialog Open(string windowName, string content, Action sureAction, Vector3 startPoint)
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

            Vector2 anchorPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uIDialog.GetComponent<RectTransform>(),
                startPoint, Game.Instance.UICamera, out anchorPos);

            uIDialog.btnRect.anchoredPosition = anchorPos + new Vector2(-74, 0);

            return uIDialog;
        }


        public void SetPerson(Person person)
        {
            if (headImg == null || nameText == null) return;
            headImg.sprite = GameRenderHelper.LoadHeadIcon(person.headIconID, 1);
            nameText.text = person.Name;
        }

        public static void Close()
        {
            CurInstance?.Hide();
            CurInstance = null;
        }
    }
}
