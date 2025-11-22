using Sango.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityExpedition : UGUIWindow
    {
        public UIBuildingTypeItem objUIBuildingTypeItem;
        List<UIBuildingTypeItem> landTroopTypePool = new List<UIBuildingTypeItem>();
        List<UIBuildingTypeItem> waterTroopTypePool = new List<UIBuildingTypeItem>();

        public UIPersonItem[] personItems;

        public Text landTroopTypeDescLabel;
        public Text waterTroopTypeDescLabel;


        public UITextField buildCountLabel;
        public UITextField cityGoldLabel;
        public UITextField buildingNumberLabel;

        CityExpedition cityExpeditionSys;

        bool showLand = true;

        public override void OnShow()
        {
            cityExpeditionSys = Singleton<CityExpedition>.Instance;
            showLand = true;

            int slotLength = cityExpeditionSys.ActivedLandTroopTypes.Count;
            while (landTroopTypePool.Count < slotLength)
            {
                GameObject go = GameObject.Instantiate(objUIBuildingTypeItem.gameObject, objUIBuildingTypeItem.transform.parent);
                UIBuildingTypeItem cityBuildingSlot = go.GetComponent<UIBuildingTypeItem>();
                landTroopTypePool.Add(cityBuildingSlot);
                cityBuildingSlot.onSelected = OnSelectLandType;
                go.SetActive(true);
            }

            for (int i = slotLength; i < landTroopTypePool.Count; i++)
                landTroopTypePool[i].gameObject.SetActive(false);

            for (int i = 0; i < slotLength; i++)
            {
                TroopType troopType = cityExpeditionSys.ActivedLandTroopTypes[i];
                UIBuildingTypeItem cityBuildingSlot = landTroopTypePool[i];
                cityBuildingSlot.SetTroopType(troopType).SetIndex(i).SetSelected(cityExpeditionSys.CurSelectLandTrropTypeIndex == i);
            }

            slotLength = cityExpeditionSys.ActivedWaterTroopTypes.Count;
            while (waterTroopTypePool.Count < slotLength)
            {
                GameObject go = GameObject.Instantiate(objUIBuildingTypeItem.gameObject, objUIBuildingTypeItem.transform.parent);
                UIBuildingTypeItem cityBuildingSlot = go.GetComponent<UIBuildingTypeItem>();
                waterTroopTypePool.Add(cityBuildingSlot);
                cityBuildingSlot.onSelected = OnSelectLandType;
                go.SetActive(true);
            }

            for (int i = slotLength; i < waterTroopTypePool.Count; i++)
                waterTroopTypePool[i].gameObject.SetActive(false);

            for (int i = 0; i < slotLength; i++)
            {
                TroopType troopType = cityExpeditionSys.ActivedWaterTroopTypes[i];
                UIBuildingTypeItem cityBuildingSlot = waterTroopTypePool[i];
                cityBuildingSlot.SetTroopType(troopType).SetIndex(i).SetSelected(cityExpeditionSys.CurSelectWaterTrropTypeIndex == i);
            }

            UpdateContent();
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void OnCancel()
        {
            cityExpeditionSys.Done();
        }

        public void UpdateContent()
        {
            for (int i = 0; i < personItems.Length; ++i)
            {
                if (i < cityExpeditionSys.personList.Count)
                    personItems[i].SetPerson(cityExpeditionSys.personList[i]);
                else
                    personItems[i].SetPerson(null);
            }

            UpdateTroopStatus();
            
        }

        void UpdateTroopStatus()
        {
            int atk, def, intel, build, move;
            bool hasPeson = cityExpeditionSys.personList.Count > 0;
            if (showLand)
            {
                if(hasPeson)
                {
                    atk = cityExpeditionSys.TargetTroop.landAttack;
                    def = cityExpeditionSys.TargetTroop.landDefence;
                    intel = cityExpeditionSys.TargetTroop.Intelligence;
                    build = cityExpeditionSys.TargetTroop.BuildPower;
                    move = cityExpeditionSys.TargetTroop.landMoveAbility;
                }
                else
                {
                    atk = cityExpeditionSys.TargetTroop.LandTroopType.atk;
                    def = cityExpeditionSys.TargetTroop.LandTroopType.def;
                    intel = cityExpeditionSys.TargetTroop.Intelligence;
                    build = cityExpeditionSys.TargetTroop.BuildPower;
                    move = cityExpeditionSys.TargetTroop.LandTroopType.move;
                }
            }
            else
            {
                if (hasPeson)
                {
                    atk = cityExpeditionSys.TargetTroop.waterAttack;
                    def = cityExpeditionSys.TargetTroop.waterDefence;
                    intel = cityExpeditionSys.TargetTroop.Intelligence;
                    build = cityExpeditionSys.TargetTroop.BuildPower;
                    move = cityExpeditionSys.TargetTroop.waterMoveAbility;
                }
                else
                {
                    atk = cityExpeditionSys.TargetTroop.WaterTroopType.atk;
                    def = cityExpeditionSys.TargetTroop.WaterTroopType.def;
                    intel = cityExpeditionSys.TargetTroop.Intelligence;
                    build = cityExpeditionSys.TargetTroop.BuildPower;
                    move = cityExpeditionSys.TargetTroop.WaterTroopType.move;
                }
            }
        }

        public void OnSelectWaterType(UIBuildingTypeItem buildingTypeItem)
        {
            if (cityExpeditionSys.CurSelectWaterTrropTypeIndex >= 0)
                waterTroopTypePool[cityExpeditionSys.CurSelectWaterTrropTypeIndex].SetSelected(false);
            cityExpeditionSys.CurSelectWaterTrropTypeIndex = buildingTypeItem.index;
            buildingTypeItem.SetSelected(true);
            cityExpeditionSys.UpdateJobValue();
            UpdateContent();
        }

        public void OnSelectLandType(UIBuildingTypeItem buildingTypeItem)
        {
            if (cityExpeditionSys.CurSelectLandTrropTypeIndex >= 0)
                landTroopTypePool[cityExpeditionSys.CurSelectLandTrropTypeIndex].SetSelected(false);
            cityExpeditionSys.CurSelectLandTrropTypeIndex = buildingTypeItem.index;
            buildingTypeItem.SetSelected(true);
            cityExpeditionSys.UpdateJobValue();
            UpdateContent();
        }

        public void OnPersonChange(List<Person> personList)
        {
            cityExpeditionSys.personList = personList;
            cityExpeditionSys.UpdateJobValue();
           
            UpdateContent();
        }

        public void OnSelectPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(cityExpeditionSys.TargetCity.freePersons,
                cityExpeditionSys.personList, 3, OnPersonChange, cityExpeditionSys.customTitleList, cityExpeditionSys.customTitleName);
        }
    }
}
