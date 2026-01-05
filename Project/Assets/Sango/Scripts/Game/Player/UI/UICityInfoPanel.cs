using Sango.Game.Player;
using Sango.Loader;
using Sango.Render;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityInfoPanel : UGUIWindow
    {
        public RectTransform root;
        public Text name;
        public UITextField gold;
        public UITextField food;

        public UITextField security;
        public UITextField agriculture;
        public UITextField commerce;
        public UITextField population;
        public UITextField troopPopulation;
        public UITextField popularSupport;
        public UITextField buildingCount;
        public UITextField durability;

        public UITextField troops;
        public UITextField woundedTroops;
        public UITextField morale;

        public UITextField[] itemLabel;

        public UITextField goldIncome;
        public UITextField foodIncome;
        public UITextField business;

        public UITextField person;
        public UITextField captive;
        public UITextField wild;

        public override void OnShow(params object[] objects)
        {
            base.OnShow();
            City city = (City)objects[0];

            float dep = (float)Screen.width / Screen.height;
            Vector2 pos = Camera.main.WorldToScreenPoint(city.CenterCell.Position);
            bool showLeft = pos.x > (Screen.width / 2 - 100);
            pos = root.anchoredPosition;
            if (showLeft)
                pos.x = -(dep * 1080f - root.sizeDelta.x) / 2;
            else
                pos.x = (dep * 1080f - root.sizeDelta.x) / 2;
            root.anchoredPosition = pos;

            name.text = city.Name;
            gold.SetText(city.gold.ToString());
            food.SetText(city.food.ToString());
            security.SetText(city.security.ToString());
            agriculture.SetText(city.agriculture.ToString());
            commerce.SetText(city.commerce.ToString());
            population.SetText(city.population.ToString());
            troopPopulation.SetText(city.troopPopulation.ToString());
            popularSupport.SetText(city.popularSupport.ToString());
            buildingCount.SetText($"{city.GetInteriorCellUsedCount()}/{city.InteriorCellCount}");
            durability.SetText(city.durability.ToString());

            for (int i = 0; i < 4; i++)
            {
                itemLabel[i].SetText(city.itemStore.GetNumber(i + 2).ToString());
            }

            itemLabel[4].SetText(city.itemStore.GetNumber(7).ToString());
            itemLabel[5].SetText(city.itemStore.GetNumber(9).ToString());
            itemLabel[6].SetText(city.itemStore.GetNumber(12).ToString());

            troops.SetText(city.troops.ToString());
            woundedTroops.SetText(city.woundedTroops.ToString());
            morale.SetText(city.morale.ToString());

            goldIncome.SetText(city.totalGainGold.ToString());
            foodIncome.SetText(city.totalGainFood.ToString());
            business.SetText(city.hasBusiness.ToString());

            person.SetText($"{city.FreePersonCount}/{city.allPersons.Count}");
            captive.SetText(city.captiveList.Count.ToString());
            wild.SetText(city.wildPersons.Count.ToString());
        }

        public override void OnHide()
        {
            base.OnHide();
        }
    }
}
