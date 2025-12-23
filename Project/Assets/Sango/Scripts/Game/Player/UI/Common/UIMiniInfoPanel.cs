using Sango.Loader;
using Sango.Render;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIMiniInfoPanel : MonoBehaviour
    {
        public RectTransform root;
        public Text nameLabel;
        public Image forceColor;
        public Text forceName;
        public Image corpsColor;
        public Text corpsId;

        public void SetCorps(Corps corps)
        {
            if (corps != null)
            {
                forceName.text = corps.BelongForce.Name;
                forceColor.color = corps.BelongForce.Color;
                corpsColor.color = corps.Color;
                corpsId.text = corps.Index.ToString();
            }
            else
            {
                forceName.text = "----";
                forceColor.color = Color.white;
                corpsColor.color = Color.white;
                corpsId.text = "-";
            }
        }

    }
}