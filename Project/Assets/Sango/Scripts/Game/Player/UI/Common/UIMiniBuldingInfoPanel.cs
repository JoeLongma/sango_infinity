using Sango.Loader;
using Sango.Render;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIMiniBuldingInfoPanel : UIMiniInfoPanel
    {
        public UITextField durability;

        public UIMiniBuldingInfoPanel Show(Building c)
        {
            nameLabel.text = c.Name;
            SetCorps(c.BelongCorps);
            durability.text = $"{c.durability}/{c.DurabilityLimit}";
            return this;
        }

    }
}