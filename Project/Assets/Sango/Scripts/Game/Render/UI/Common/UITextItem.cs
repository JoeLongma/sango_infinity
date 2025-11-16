using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UITextItem : MonoBehaviour
    {
        public Text label;

        public void SetWidth(int width)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
            LayoutElement layoutElement = GetComponent<LayoutElement>();
            if (layoutElement != null)
                layoutElement.preferredWidth = width;
        }

        public void SetText(string lab)
        {
            label.text = lab;
        }

        public void SetColor(Color c)
        {
            label.color = c;
        }

        public void SetAlignment(TextAnchor c)
        {
            label.alignment = c;
        }
    }
}