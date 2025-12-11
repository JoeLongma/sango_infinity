using Newtonsoft.Json.Utilities.LinqBridge;
using Sango.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityTransport : UGUIWindow
    {
        public UIItemTypeRect itemTypeRect;
        public UIItemTypeSliderRect itemTypeSliderRect;

        public UIPersonItem[] personItems;

        public UITextField troopsLabel;
        public UITextField goldLabel;
        public UITextField foodLabel;
        public UITextField dayTurnLabel;

        public UITextField itemTroopsLabel;
        public UITextField itemGoldLabel;
        public UITextField itemFoodLabel;

        public Slider troopsSlider;
        public Slider goldSlider;
        public Slider foodSlider;

        CityTransport cityTransportSys;
        Dictionary<int, UIItemType> id2UIItemType = new Dictionary<int, UIItemType>();
        City targetCity;
        Troop targetTroop;
        public override void OnShow()
        {
            cityTransportSys = Singleton<CityTransport>.Instance;
            targetCity = cityTransportSys.TargetCity;
            targetTroop = cityTransportSys.TargetTroop;

            itemTypeRect.onItemTypeShow = OnItemTypeShow;
            itemTypeSliderRect.onItemTypeShow = OnItemTypeSliderShow;

            UpdateContent();
        }

        void OnItemTypeShow(ItemType itemType, UIItemType uIItemType)
        {
            id2UIItemType.Add(itemType.Id, uIItemType);
            uIItemType.SetItemType(itemType);
            uIItemType.SetNumber(targetCity.itemStore.GetNumber(itemType.Id));
            int troopHas = targetTroop.itemStore.GetNumber(itemType.Id);
            uIItemType.SetUsed(troopHas);
        }

        void OnItemTypeSliderShow(ItemType itemType, UIItemTypeSlider uIItemTypeSlider)
        {
            int itemNumber = targetCity.itemStore.GetNumber(itemType.Id);
            if (itemNumber <= 0)
            {
                uIItemTypeSlider.SetValid(false);
                uIItemTypeSlider.numberSlider.SetValueWithoutNotify(0);
                uIItemTypeSlider.numberLabel.text = "0/0";
                return;
            }
            uIItemTypeSlider.SetValid(true);

            int troopHas = targetTroop.itemStore.GetNumber(itemType.Id);
            uIItemTypeSlider.numberSlider.SetValueWithoutNotify(troopHas / itemNumber);
            uIItemTypeSlider.numberLabel.text = $"{troopHas}/{itemNumber}";
            uIItemTypeSlider.numberSlider.onValueChanged.RemoveAllListeners();
            uIItemTypeSlider.numberSlider.onValueChanged.AddListener((p) =>
            {
                int num = (int)Math.Ceiling(itemNumber * p);
                uIItemTypeSlider.numberLabel.text = $"{num}/{itemNumber}";
                targetTroop.itemStore.Set(itemType.Id, num);

                UIItemType uIItemType = id2UIItemType[itemType.Id];
                uIItemType.SetUsed(num);
            });
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void OnCancel()
        {
            cityTransportSys.Done();
        }

        public void OnOK()
        {
            cityTransportSys.MakeTroop();
        }

        public void OpenNumberPanel_troops()
        {

        }

        public void OpenNumberPanel_gold()
        {

        }

        public void OpenNumberPanel_food()
        {

        }

        public void OnTroopsSliderValueChanged(float p)
        {
            if (cityTransportSys.personList.Count == 0)
                return;

            int troop = (int)Math.Ceiling(targetCity.troops * p);
            targetTroop.troops = troop;
            UpdateTroopsInfo();
        }

        public void OnGoldSliderValueChanged(float p)
        {
            if (cityTransportSys.personList.Count == 0)
                return;

            int gold = (int)Math.Ceiling(targetCity.gold * p);
            targetTroop.gold = gold;
            UpdateTroopsInfo();
        }

        public void OnFoodSliderValueChanged(float p)
        {
            if (cityTransportSys.personList.Count == 0)
                return;

            int food = (int)Math.Ceiling(targetCity.food * p);
            targetTroop.food = food;
            UpdateTroopsInfo();
        }

        public void UpdateContent()
        {
            id2UIItemType.Clear();
            itemTypeRect.Init();
            itemTypeSliderRect.Init();

            for (int i = 0; i < personItems.Length; ++i)
            {
                if (i < cityTransportSys.personList.Count)
                    personItems[i].SetPerson(cityTransportSys.personList[i]);
                else
                    personItems[i].SetPerson(null);
            }

            UpdateTroopsInfo();
        }

        void SetItemLabel(UITextField label, int all, int ues)
        {
            int left = all - ues;
            if (left > 0)
            {
                if (ues == 0)
                    label.text = all.ToString();
                else
                    label.text = $"{all} → {left}";
            }
            else
            {
                if (all == 0)
                    label.text = $"<color=#ff0000>{all}</color>";
                else
                    label.text = $"{all}→<color=#ff0000>{left}</color>";
            }
        }

        void UpdateTroopsInfo()
        {
            troopsSlider.SetValueWithoutNotify((float)targetTroop.troops / targetCity.troops);
            goldSlider.SetValueWithoutNotify((float)targetTroop.gold / targetCity.gold);
            foodSlider.SetValueWithoutNotify((float)targetTroop.food / targetCity.food);
            troopsLabel.text = $"{targetTroop.troops}/{targetCity.troops}";
            goldLabel.text = $"{targetTroop.gold}/{targetCity.gold}";
            foodLabel.text = $"{targetTroop.food}/{targetCity.food}";
            int turnCount = (int)(targetTroop.food / (targetTroop.troops * Scenario.Cur.Variables.baseFoodCostInTroop));
            dayTurnLabel.text = $"{turnCount * 10}日";

            SetItemLabel(itemTroopsLabel, targetCity.troops, targetTroop.troops);
            SetItemLabel(itemGoldLabel, targetCity.gold, targetTroop.gold);
            SetItemLabel(itemFoodLabel, targetCity.food, targetTroop.food);
        }

        public void OnPersonChange(List<Person> personList)
        {
            cityTransportSys.personList = personList;
            cityTransportSys.UpdateJobValue();

            UpdateContent();
        }

        public void OnSelectPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(cityTransportSys.TargetCity.freePersons,
                cityTransportSys.personList, 3, OnPersonChange, cityTransportSys.customTitleList, cityTransportSys.customTitleName);
        }

        public void OnSlecteMax()
        {
            targetTroop.itemStore = targetCity.itemStore.Copy();
            targetTroop.troops = targetCity.troops;
            targetTroop.food = targetCity.food;
            targetTroop.gold = targetCity.gold;
            UpdateContent();
        }

        public void OnSlecteMin()
        {
            targetTroop.itemStore.Clear();
            targetTroop.troops = 0;
            targetTroop.food = 0;
            targetTroop.gold = 0;
            UpdateContent();
        }
    }
}
