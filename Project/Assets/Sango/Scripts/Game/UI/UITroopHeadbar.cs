using Sango.Loader;
using UnityEngine.UI;

namespace Sango.Game.UI
{
    public class UITroopHeadbar : UGUIWindow
    {
        public Image headIcon;
        public Text name;
        public Image state;
        public Image food;
        public Image energy;
        public Image angry;
        public Text number;

        public void Init(Troop troop)
        {
            name.text = troop.Name;
            headIcon.sprite = UIHelper.LoadHeadIcon(troop.Leader.headIconID);
            UpdateState(troop);
        }

        public void UpdateState(Troop troop)
        {
            state.enabled = false;
            energy.fillAmount = (float)troop.energy / 100.0f;
            angry.fillAmount = 0;
            number.text = troop.troops.ToString();
        }
    }
}
