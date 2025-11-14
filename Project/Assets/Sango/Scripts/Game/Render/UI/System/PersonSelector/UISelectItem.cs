using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UISelectItem : MonoBehaviour
    {
        public Image select;

        public UISelectItem SetSelected(bool b)
        {
            select.enabled = b;
            return this;
        }
        public UISelectItem SetWidth(int width)
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