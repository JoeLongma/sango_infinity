using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace HSVPicker
{
    [RequireComponent(typeof(TMP_InputField))]
    public class HexColorField : MonoBehaviour
    {
        public ColorPicker hsvpicker;

        public bool displayAlpha;

        private TMP_InputField hexInputField;

        private void Awake()
        {
            hexInputField = GetComponent<TMP_InputField>();

            // 添加监听器以保持文本（和颜色）保持最新
            hexInputField.onEndEdit.AddListener(UpdateColor);
            hsvpicker.onValueChanged.AddListener(UpdateHex);
        }

        private void OnDestroy()
        {
            hexInputField.onValueChanged.RemoveListener(UpdateColor);
            hsvpicker.onValueChanged.RemoveListener(UpdateHex);
        }

        private void UpdateHex(Color newColor)
        {
            hexInputField.text = ColorToHex(newColor);
        }

        private void UpdateColor(string newHex)
        {
            Color color;
            if (!newHex.StartsWith("#"))
                newHex = "#"+newHex;
            if (ColorUtility.TryParseHtmlString(newHex, out color))
                hsvpicker.CurrentColor = color;
            else
                Debug.Log("十六进制值格式不正确，有效格式包括：#RGB、 #RGBA、#RRGGBB 和 #RRGGBBAA（# 是可选的）");
        }

        private string ColorToHex(Color32 color)
        {
            return displayAlpha
                ? string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.r, color.g, color.b, color.a)
                : string.Format("#{0:X2}{1:X2}{2:X2}", color.r, color.g, color.b);
        }
    }
}