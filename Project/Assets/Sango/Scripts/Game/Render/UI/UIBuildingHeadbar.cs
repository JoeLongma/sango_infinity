using Sango.Loader;
using UnityEngine.UI;
namespace Sango.Game.Render.UI

{
    public class UIBuildingHeadbar : UGUIWindow
    {
        public Text name;
        public Image durability;
        public AnimationText aniText;
        public void Init(Building building)
        {
            name.text = building.Name;
            aniText.Clear();
            UpdateState(building);
        }

        public void UpdateState(Building building)
        {
            durability.fillAmount = (float)building.durability / building.DurabilityLimit;
        }

        public void ShowDamage(int damage, int damageType = 0)
        {
            UITools.ShowDamage(aniText, damage, damageType);
        }

    }
}
