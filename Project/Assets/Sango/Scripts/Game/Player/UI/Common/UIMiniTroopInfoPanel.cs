using Sango.Loader;
using Sango.Render;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIMiniTroopInfoPanel : UIMiniInfoPanel
    {
        public Image icon;
        public UITextField person1;
        public UITextField person2;
        public UITextField gold;
        public UITextField food;
        public UITextField morale;
        public UITextField troops;
        public UITextField troopType;
        public UITextField troopTypeLevel;

        public UIMiniTroopInfoPanel Show(Troop c)
        {
            nameLabel.text = c.Name;
            SetCorps(c.BelongCorps);

            person1.text = c.Member1 != null ? c.Member1.Name : "";
            person2.text = c.Member2 != null ? c.Member2.Name : "";
            gold.text = c.gold.ToString();
            food.text = c.food.ToString();
            morale.text = $"{c.morale}/{c.MaxMorale}";
            troops.text = c.troops.ToString();
            troopType.text = c.TroopType.Name;
            troopTypeLevel.text = Scenario.Cur.Variables.GetAbilityName(c.TroopTypeLv);
            return this;
        }

    }
}