using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIFloatSliderField : MonoBehaviour
    {
        public Text title;
        public Slider slider;
        public InputField inputField;

        public float min;
        public float max;
        public float value;

        public System.Action<float> onValueChange;

        public void Set(string title, float value, float min, float max, System.Action<float> onValueChange)
        {
            this.title.text = title;
            if (min >= max)
            {
                Sango.Log.Error("min >= max! set min = max - 1");
                min = max - 1;
            }
            this.slider.minValue = min;
            this.slider.maxValue = max;
            this.slider.SetValueWithoutNotify((float)(value - min) / (float)(max - min));
            inputField.SetTextWithoutNotify(value.ToString());
            inputField.characterValidation = InputField.CharacterValidation.Integer;
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(OnInputNumberChanged);
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(OnSliderNumberChanged);
            this.onValueChange = onValueChange;
        }

        void OnInputNumberChanged(string number)
        {
            if (float.TryParse(number, out float v))
            {
                if (v > max) v = max;
                else if (v < min) v = min;

                value = v;
                slider.SetValueWithoutNotify((float)(value - min) / (max - min));
                onValueChange?.Invoke(value);
            }
        }

        void OnSliderNumberChanged(float number)
        {
            value = min + (max - min) * number;
            inputField.SetTextWithoutNotify(value.ToString());
            onValueChange?.Invoke(value);
        }
    }
}