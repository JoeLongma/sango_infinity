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
        public UITextField product;
        public UITextField[] worker;

        public UIMiniBuldingInfoPanel Show(Building c)
        {
            nameLabel.text = c.Name;
            SetCorps(c.BelongCorps);
            durability.text = $"{c.durability}/{c.DurabilityLimit}";

            for (int i = 0; i < worker.Length; i++)
            {
                UITextField uIWorker = worker[i];
                uIWorker.gameObject.SetActive((!c.isComplate && i < c.Workers.Count) || i < c.BuildingType.workerLimit);
                if (c.Workers != null && i < c.Workers.Count)
                {
                    uIWorker.text = c.Workers[i].Name;
                }
                else
                {
                    uIWorker.text = "--";
                }
            }

            return this;
        }

    }
}