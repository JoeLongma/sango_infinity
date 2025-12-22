using Sango.Loader;
using Sango.Render;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIMiniPortInfoPanel : UIMiniInfoPanel
    {
        public Image icon;
        public UITextField leader;
        public UITextField gold;
        public UITextField food;
        public UITextField durability;
        public UITextField troops;
        public UITextField persons;

        public UIMiniPortInfoPanel Show(City c)
        {
            nameLabel.text = c.Name;
            SetCorps(c.BelongCorps);
            leader.text = c.Leader != null ? c.Leader.Name : "----";
            gold.text = c.gold.ToString();
            food.text = c.food.ToString();
            durability.text = $"{c.durability}/{c.DurabilityLimit}";
            troops.text = c.troops.ToString();
            persons.text = $"{c.allPersons.Count - c.FreePersonCount}/{c.allPersons.Count}";
            return this;
        }

    }
}