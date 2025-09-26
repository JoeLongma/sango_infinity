using Sango.Loader;
using UnityEngine.UI;

namespace Sango.Game.Render.UI

{
    public class UICityHeadbar : UGUIWindow
    {
        public Text name;
        public Image state;
        public Image food;
        public Image durability;
        public Text number;

        public void Init(City city)
        {
            name.text = city.Name;
            UpdateState(city);
        }

        public void UpdateState(City city)
        {
            state.enabled = false;
            food.enabled = false;
            durability.fillAmount = (float)city.durability / city.CityLevelType.maxDurability;
            number.text = city.troops.ToString();
        }
    }
}
