using Sango.Loader;
using Sango.Render;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIMiniCityInfoPanel : UIMiniInfoPanel
    {
        public Image icon;
        public UITextField leader;
        public UITextField gold;
        public UITextField food;
        public UITextField security;
        public UITextField durability;
        public UITextField troops;
        public UITextField persons;

        public UIMiniCityInfoPanel Show(City c)
        {
            nameLabel.text = c.Name;
            SetCorps(c.BelongCorps);
            leader.text = c.Leader != null ? c.Leader.Name : "----";
            gold.text = c.gold.ToString();
            food.text = c.food.ToString();
            security.text = c.security.ToString();
            durability.text = $"{c.durability}/{c.DurabilityLimit}";
            troops.text = c.troops.ToString();
            persons.text = $"{c.FreePersonCount}/{c.allPersons.Count}";
            return this;
        }

    }
}