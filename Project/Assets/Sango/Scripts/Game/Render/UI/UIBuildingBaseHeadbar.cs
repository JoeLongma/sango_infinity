using Sango.Loader;
using UnityEngine.UI;
namespace Sango.Game.Render.UI

{
    public class UIBuildingBaseHeadbar : UGUIWindow
    {
        public Text name;
        public Image durability;
        public AnimationText aniText;
        public virtual void Init(BuildingBase building)
        {
            if (name != null)
                name.text = building.Name;
            if (aniText != null)
                aniText.Clear();
            UpdateState(building);
        }

        public virtual void UpdateState(BuildingBase building)
        {
            if (durability == null) return;
            durability.fillAmount = (float)building.durability / building.DurabilityLimit;
        }

        public virtual void ShowDamage(int damage, int damageType = 13)
        {
            UITools.ShowDamage(aniText, damage, damageType);
        }

    }
}
