using Sango.Game.Player;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityInformation : UGUIWindow
    {
        public Text windiwTitle;

        public Toggle[] tabs;

        public UIObjectList uIObjectList;

        public UIPersonItem cityLeaderPersonItems;
        public UIStatusItem cityLeaderStatusItem;

        public UITextField cityNameLabel;

        public UITextField forceNameLabel;
        public UITextField cityPersonCountLabel;
        public UITextField corpsNameLabel;
        public UITextField captiveCountLabel;
        public UITextField wildCountLabel;
        public UITextField emperorLabel;

        public UIScenarioCityMap scenarioCityMap;
        public UITextField goldLabel;
        public UITextField foodLabel;
        public UITextField troopsLabel;
        public UITextField gold_costLabel;
        public UITextField food_costLabel;
        public UITextField moraleLabel;
        public UITextField gold_gainLabel;
        public UITextField food_gainLabel;
        public UITextField gold_wonderLabel;
        public UITextField food_wonderLabel;
        public UITextField gold_enoughLabel;
        public UITextField food_enoughLabel;
        public UITextField warLabel;
        public UITextField durabilityLabel;
        public UITextField empty_fieldLabel;
        public UITextField securityLabel;
        public UITextField bussinessLabel;
        public UITextField featureLabel;
        public Button[] disaster_icon_list;

        public UIBuildingTypeItem boatItem;
        public UIBuildingTypeItem itemObject;
        CreatePool<UIBuildingTypeItem> itemPool;

        public UIBuildingTypeItem buildingItemObject;
        public UITextField buildingNumLabel;
        CreatePool<UIBuildingTypeItem> buildingPool;

        City TargetCity;
        CityInformation currentSystem;

        bool items_inited = false;
        bool buildings_inited = false;

        public Button port_gate_btn;
        public Button troop_btn;
        public Button person_btn;

        protected override void Awake()
        {
            buildingPool = new CreatePool<UIBuildingTypeItem>(buildingItemObject);
            itemPool = new CreatePool<UIBuildingTypeItem>(itemObject);
        }

        public override void OnShow()
        {
            currentSystem = GameSystem.GetSystem<CityInformation>();
            TargetCity = currentSystem.TargetCity;
            uIObjectList.Init(currentSystem.all_citits, CitySortFunction.SortByName, OnObjectSelected);
            uIObjectList.SelectDefaultObject(TargetCity);
            windiwTitle.text = currentSystem.Name;
            tabs[0].isOn = true;
            ShowCity(TargetCity);
        }

        void OnObjectSelected(int index)
        {
            ShowCity(currentSystem.all_citits[index] as City);
        }

        public void ShowCity(City city)
        {
            TargetCity = city;
            items_inited = false;
            buildings_inited = false;

            cityNameLabel.text = city.Name;
            cityLeaderPersonItems.SetPerson(city.Leader);
            cityLeaderStatusItem.SetPerson(city.Leader);

            forceNameLabel.text = CitySortFunction.SortByBelongForce.GetValueStr(city);
            cityPersonCountLabel.text = CitySortFunction.SortByAllPersonCountInfo.GetValueStr(city);
            corpsNameLabel.text = CitySortFunction.SortByBelongCorps.GetValueStr(city);
            captiveCountLabel.text = CitySortFunction.SortByCaptiveCount.GetValueStr(city);
            wildCountLabel.text = CitySortFunction.SortByWildCount.GetValueStr(city);

            port_gate_btn.interactable = (city.portList.Count + city.gateList.Count) > 0;
            troop_btn.interactable = city.allTroops.Count > 0;
            person_btn.interactable = city.allPersons.Count > 0;

            // TODO: 皇帝
            emperorLabel.text = "";
            UpdateCityStateContent();
        }

        void UpdateCityStateContent()
        {
            City city = TargetCity;
            Scenario scenario = Scenario.Cur;
            scenarioCityMap.Show(scenario, city);
            goldLabel.text = $"{city.gold}/{city.GoldLimit}";
            foodLabel.text = $"{city.food}/{city.FoodLimit}";
            troopsLabel.text = $"{city.troops}/{city.TroopsLimit}";
            moraleLabel.text = $"{city.morale}/{city.MaxMorale}";

            gold_costLabel.text = city.GoldCost(scenario).ToString();
            food_costLabel.text = city.FoodCost(scenario).ToString();
            gold_gainLabel.text = city.totalGainGold.ToString();
            food_gainLabel.text = city.totalGainFood.ToString();

            gold_wonderLabel.text = "";
            food_wonderLabel.text = "";
            gold_enoughLabel.text = "";
            food_enoughLabel.text = "";

            warLabel.text = city.EnemyCount > 0 ? GameDefine.o : GameDefine.x;
            durabilityLabel.text = $"{city.durability}/{city.DurabilityLimit}";
            empty_fieldLabel.text = $"{city.InteriorCellCount - city.GetInteriorCellUsedCount()}/{city.InteriorCellCount}";
            securityLabel.text = city.security.ToString();
            bussinessLabel.text = CitySortFunction.SortByHasBusiness.GetValueStr(city);
            //featureLabel.text = city.security.ToString();
            //disaster_icon_list.text = city.security.ToString();
        }

        void UpdateCityItemsContent()
        {
            if (items_inited) return;
            itemPool.Reset();
            List<ItemType> ItemTypes = TargetCity.BelongForce.createdItemTypes;
            int len = ItemTypes.Count;
            for (int i = 0; i < len; i++)
            {
                ItemType itemType = ItemTypes[i];
                int totalNum = TargetCity.itemStore.GetNumber(itemType.Id);
                if (itemType.subKind == (int)ItemSubKindType.Boat)
                {
                    boatItem.SetItemType(itemType).SetIndex(i).SetNum(totalNum);
                }
                else
                {
                    UIBuildingTypeItem cityBuildingSlot = itemPool.Create();
                    cityBuildingSlot.SetItemType(itemType).SetIndex(i).SetNum(totalNum);
                }
            }
        }

        void UpdateCityBuildingsContent()
        {
            if (buildings_inited) return;
            City city = TargetCity;
            buildingPool.Reset();
            buildingNumLabel.text = $"{city.InteriorCellCount - city.GetInteriorCellUsedCount()}/{city.InteriorCellCount}";
            Dictionary<BuildingType, int> building_num = new Dictionary<BuildingType, int>();
            city.allBuildings.ForEach(b =>
            {
                if (!b.IsIntorBuilding()) return;
                int num;
                if (building_num.TryGetValue(b.BuildingType, out num))
                {
                    building_num[b.BuildingType] = num + 1;
                }
                else
                {
                    building_num[b.BuildingType] = 1;
                }
            });

            foreach (var buildingType in building_num.Keys)
            {
                UIBuildingTypeItem item = buildingPool.Create();
                item.SetBuildingType(buildingType).nameLabel.text = $"{buildingType.Name}（{building_num[buildingType]}）";
            }

        }

        public void OnCancel()
        {
            currentSystem.Exit();
        }

        public void OnCityStateTab(bool b)
        {

        }

        public void OnCityItemsTab(bool b)
        {
            if (b)
            {
                UpdateCityItemsContent();
            }
        }

        public void OnCityBuildingsTab(bool b)
        {
            if (b)
            {
                UpdateCityBuildingsContent();
            }
        }

        public void OnGatePortButton()
        {

        }

        public void OnTroopButton()
        {

        }

        public void OnPersonButton()
        {

        }
    }
}
