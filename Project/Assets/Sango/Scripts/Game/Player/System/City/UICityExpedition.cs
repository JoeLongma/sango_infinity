using Sango.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityExpedition : UGUIWindow
    {
        public UIBuildingTypeItem objUIBuildingTypeItemLand;
        public UIBuildingTypeItem objUIBuildingTypeItemWater;
        List<UIBuildingTypeItem> landTroopTypePool = new List<UIBuildingTypeItem>();
        List<UIBuildingTypeItem> waterTroopTypePool = new List<UIBuildingTypeItem>();

        public UIPersonItem[] personItems;

        public UITextField landTroopTypeDescLabel;
        public UITextField waterTroopTypeDescLabel;


        public UITextField troopsLabel;
        public UITextField goldLabel;
        public UITextField foodLabel;
        public UITextField dayTurnLabel;


        public UITextField atkLaebl;
        public UITextField defLaebl;
        public UITextField intLaebl;
        public UITextField buildLaebl;
        public UITextField moveLaebl;
        public UITextField typeLaebl;
        public UITextField abilityLaebl;
        public UITextField energyLaebl;
        public UITextField[] skillLabel;

        public UITextField[] itemLabels;

        public UITextField itemTroopsLabel;
        public UITextField itemGoldLabel;
        public UITextField itemFoodLabel;

        public Slider troopsSlider;
        public Slider goldSlider;
        public Slider foodSlider;

        CityExpedition cityExpeditionSys;

        bool showLand = true;

        public override void OnShow()
        {
            cityExpeditionSys = Singleton<CityExpedition>.Instance;
            showLand = true;

            int slotLength = cityExpeditionSys.ActivedLandTroopTypes.Count;
            while (landTroopTypePool.Count < slotLength)
            {
                GameObject go = GameObject.Instantiate(objUIBuildingTypeItemLand.gameObject, objUIBuildingTypeItemLand.transform.parent);
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
                GameObject go = GameObject.Instantiate(objUIBuildingTypeItemWater.gameObject, objUIBuildingTypeItemWater.transform.parent);
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

        public void OnOK()
        {
            cityExpeditionSys.MakeTroop();
        }

        public void OnBuildTroopType1()
        {

        }

        public void OnBuildTroopType2()
        {

        }

        public void OnBuildTroopType3()
        {

        }

        public void OnBuildTroopType4()
        {

        }

        public void OnBuildTroopType5()
        {

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

        public void OnTroopTypeShowLand(bool b)
        {

        }

        public void OnTroopTypeShowWater(bool b)
        {

        }

        public void OnTroopsSliderValueChanged(float p)
        {

        }

        public void OnGoldSliderValueChanged(float p)
        {

        }

        public void OnFoodSliderValueChanged(float p)
        {

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
            Troop targetTroop = cityExpeditionSys.TargetTroop;
            if (showLand)
            {
                if (hasPeson)
                {
                    atk = targetTroop.landAttack;
                    def = targetTroop.landDefence;
                    intel = targetTroop.Intelligence;
                    build = targetTroop.BuildPower;
                    move = targetTroop.landMoveAbility;
                }
                else
                {
                    atk = targetTroop.LandTroopType.atk;
                    def = targetTroop.LandTroopType.def;
                    intel = targetTroop.Intelligence;
                    build = targetTroop.BuildPower;
                    move = targetTroop.LandTroopType.move;
                }

                typeLaebl.text = targetTroop.LandTroopType.Name;
                abilityLaebl.text = Scenario.Cur.Variables.GetAbilityName(targetTroop.LandTroopTypeLv);
            }
            else
            {
                if (hasPeson)
                {
                    atk = targetTroop.waterAttack;
                    def = targetTroop.waterDefence;
                    intel = targetTroop.Intelligence;
                    build = targetTroop.BuildPower;
                    move = targetTroop.waterMoveAbility;
                }
                else
                {
                    atk = targetTroop.WaterTroopType.atk;
                    def = targetTroop.WaterTroopType.def;
                    intel = targetTroop.Intelligence;
                    build = targetTroop.BuildPower;
                    move = targetTroop.WaterTroopType.move;
                }

                typeLaebl.text = targetTroop.WaterTroopType.Name;
                abilityLaebl.text = Scenario.Cur.Variables.GetAbilityName(targetTroop.WaterTroopTypeLv);
            }

            atkLaebl.text = atk.ToString();
            defLaebl.text = def.ToString();
            intLaebl.text = intel.ToString();
            buildLaebl.text = build.ToString();
            moveLaebl.text = move.ToString();
            energyLaebl.text = targetTroop.morale.ToString();
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
