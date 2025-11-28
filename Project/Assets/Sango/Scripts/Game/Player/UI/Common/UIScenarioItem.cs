using Sango.Loader;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIScenarioItem : MonoBehaviour
    {
        public Text name;
        public Image select;
        public Button button;
        public bool IsSelected()
        {
            return select.gameObject.activeSelf;
        }

        public UIScenarioItem SetSelected(bool b)
        {
            select.gameObject.SetActive(b);
            return this;
        }

        public UIScenarioItem SetName(string n)
        {
            name.text = n;
            return this;
        }
        public UIScenarioItem BindCall(UnityAction call)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(call);
            return this;
        }

        public UIScenarioItem SetWidth(int width)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
            LayoutElement layoutElement = GetComponent<LayoutElement>();
            if (layoutElement != null)
                layoutElement.preferredWidth = width;
            return this;
        }

    }
}