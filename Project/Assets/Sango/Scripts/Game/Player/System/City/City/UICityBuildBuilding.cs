using Sango.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityBuildBuilding : UGUIWindow
    {
        public UICityBuildingSlot objUICityBuildingSlot;
        List<UICityBuildingSlot> buildingSlotPool = new List<UICityBuildingSlot>();

        public GameObject upgradeButtonObj;
        public GameObject buildButtonObj;
        public GameObject destroyButtonObj;
        public GameObject selectPersonButtonObj;

        public UIBuildingTypeItem objUIBuildingTypeItem;
        List<UIBuildingTypeItem> buildingTypeItemPool = new List<UIBuildingTypeItem>();

        public Text infoLabel;
        public Text slotInfoLabel;

        public UIPersonItem[] personItems;

        public UITextField buildCountLabel;
        public UITextField cityGoldLabel;
        public UITextField buildingNumberLabel;

        public Text buildingTypeDescLabel;

        CityBuildBuilding buildBuildingSys;

        public override void OnShow()
        {
            buildBuildingSys = Singleton<CityBuildBuilding>.Instance;
            buildBuildingSys.CurSelectSlotIndex = -1;
            int slotLength = buildBuildingSys.TargetCity.innerSlot.Length;
            while (buildingSlotPool.Count < slotLength)
            {
                GameObject go = GameObject.Instantiate(objUICityBuildingSlot.gameObject, objUICityBuildingSlot.transform.parent);
                UICityBuildingSlot cityBuildingSlot = go.GetComponent<UICityBuildingSlot>();
                buildingSlotPool.Add(cityBuildingSlot);
                //cityBuildingSlot.onSelected = OnSelectSlot;
                go.SetActive(true);
            }

            for (int i = slotLength; i < buildingSlotPool.Count; i++)
                buildingSlotPool[i].gameObject.SetActive(false);

            for (int i = 0; i < slotLength; i++)
            {
                int buildingId = buildBuildingSys.TargetCity.innerSlot[i];
                UICityBuildingSlot cityBuildingSlot = buildingSlotPool[i];
                if (buildingId > 0)
                {
                    Building building = Scenario.Cur.GetObject<Building>(buildingId);
                    //cityBuildingSlot.SetBuilding(building).SetIndex(i).SetSelected(false);
                }
                else
                {
                    if (buildBuildingSys.CurSelectSlotIndex < 0)
                        buildBuildingSys.CurSelectSlotIndex = i;
                    //cityBuildingSlot.SetBuilding(null).SetIndex(i).SetSelected(false);
                }
            }
            if (buildBuildingSys.CurSelectSlotIndex < 0) buildBuildingSys.CurSelectSlotIndex = 0;
            OnSelectSlot(buildingSlotPool[buildBuildingSys.CurSelectSlotIndex]);
            UpdateSlotInfo();
        }

        public void UpdateSlotInfo()
        {
            int total = buildBuildingSys.TargetCity.InsideSlot;
            int builded = buildBuildingSys.TargetCity.GetIntriorBuildingUsedNumber();
            slotInfoLabel.text = $"({builded}/{total})";
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void OnCancel()
        {
            buildBuildingSys.Done();
        }

        public void OnSelectSlot(UICityBuildingSlot slot)
        {
            //if (buildBuildingSys.CurSelectSlotIndex >= 0)
                //buildingSlotPool[buildBuildingSys.CurSelectSlotIndex].SetSelected(false);

            buildBuildingSys.CurSelectSlotIndex = slot.index;

            buildBuildingSys.canSelectBuildingTypes.Clear();
            // 根据建筑情况,展示其他信息
            int buildingId = buildBuildingSys.TargetCity.innerSlot[slot.index];
            if (buildingId > 0)
            {
                Building building = Scenario.Cur.GetObject<Building>(buildingId);
                if (!building.isComplte)
                {
                    destroyButtonObj.SetActive(true);
                    upgradeButtonObj.SetActive(false);
                    buildButtonObj.SetActive(false);
                    selectPersonButtonObj.SetActive(false);

                    if (building.BuildingType.nextId != 0)
                    {
                        BuildingType nextBuildingType = Scenario.Cur.GetObject<BuildingType>(building.BuildingType.nextId);
                        buildBuildingSys.canSelectBuildingTypes.Add(nextBuildingType);
                    }
                }
                else if (building.isUpgrading)
                {
                    destroyButtonObj.SetActive(true);
                    upgradeButtonObj.SetActive(false);
                    buildButtonObj.SetActive(false);
                    selectPersonButtonObj.SetActive(false);
                }
                else
                {
                    buildButtonObj.SetActive(false);
                    upgradeButtonObj.SetActive(!building.isUpgrading && building.BuildingType.nextId != 0);
                    destroyButtonObj.SetActive(true);
                    selectPersonButtonObj.SetActive(true);

                    if (building.BuildingType.nextId != 0)
                    {
                        BuildingType nextBuildingType = Scenario.Cur.GetObject<BuildingType>(building.BuildingType.nextId);
                        buildBuildingSys.canSelectBuildingTypes.Add(nextBuildingType);
                    }
                }
            }
            else
            {
                destroyButtonObj.SetActive(false);
                upgradeButtonObj.SetActive(false);
                selectPersonButtonObj.SetActive(true);
                buildButtonObj.SetActive(true);
                buildBuildingSys.canSelectBuildingTypes.Clear();
                for (int i = (int)BuildingKindType.Farm; i < (int)BuildingKindType.ArrowTower; ++i)
                {
                    buildBuildingSys.canSelectBuildingTypes.Add(Scenario.Cur.GetObject<BuildingType>(i));
                }
            }
            ShowBuildingType();
            //slot.SetSelected(true);
        }

        public void ShowBuildingType()
        {
            int len = buildBuildingSys.canSelectBuildingTypes.Count;
            while (buildingTypeItemPool.Count < len)
            {
                GameObject go = GameObject.Instantiate(objUIBuildingTypeItem.gameObject, objUIBuildingTypeItem.transform.parent);
                UIBuildingTypeItem buildingTypeItem = go.GetComponent<UIBuildingTypeItem>();
                buildingTypeItemPool.Add(buildingTypeItem);
                buildingTypeItem.onSelected = OnSelectBuildingType;
                go.SetActive(true);
            }

            for (int i = len; i < buildingTypeItemPool.Count; i++)
                buildingTypeItemPool[i].gameObject.SetActive(false);

            for (int i = 0; i < len; i++)
            {
                BuildingType buildingType = buildBuildingSys.canSelectBuildingTypes[i];
                buildingTypeItemPool[i].gameObject.SetActive(true);
                if (buildBuildingSys.CurSelectBuildingTypeIndex < 0)
                    buildBuildingSys.CurSelectBuildingTypeIndex = i;


                int totalNum = buildBuildingSys.TargetCity.GetIntriorBuildingComplateNumber(buildingType.kind);

                UIBuildingTypeItem cityBuildingSlot = buildingTypeItemPool[i];
                cityBuildingSlot.SetBuildingType(buildingType).SetIndex(i).SetSelected(false).SetNum(totalNum);
            }

            OnSelectBuildingType(buildingTypeItemPool[buildBuildingSys.CurSelectBuildingTypeIndex]);
        }

        public void OnSelectBuildingType(UIBuildingTypeItem buildingTypeItem)
        {
            if (buildBuildingSys.CurSelectBuildingTypeIndex >= 0)
            {
                buildingTypeItemPool[buildBuildingSys.CurSelectBuildingTypeIndex].SetSelected(false);
            }

            buildBuildingSys.CurSelectBuildingTypeIndex = buildingTypeItem.index;
            buildBuildingSys.TargetBuildingType = buildBuildingSys.canSelectBuildingTypes[buildingTypeItem.index];

            Person[] builder = ForceAI.CounsellorRecommendBuild(buildBuildingSys.TargetCity.freePersons, buildBuildingSys.TargetBuildingType);
            buildBuildingSys.personList.Clear();
            if (builder == null || builder.Length == 0)
            {
                for (int i = 0; i < personItems.Length; ++i)
                    personItems[i].SetPerson(null);
            }
            else
            {
                for (int i = 0; i < personItems.Length; ++i)
                {
                    if (i < builder.Length)
                    {
                        Person person = builder[i];
                        personItems[i].SetPerson(person);
                        buildBuildingSys.personList.Add(person);
                    }
                    else
                    {
                        personItems[i].SetPerson(null);
                    }
                }
            }
            buildBuildingSys.UpdateJobValue();
            buildCountLabel.text = $"{buildBuildingSys.wonderBuildCounter}回";
            cityGoldLabel.text = $"{buildBuildingSys.TargetBuildingType.cost}/{buildBuildingSys.TargetCity.gold}";

            int totalNum = buildBuildingSys.TargetCity.GetIntriorBuildingComplateNumber(buildBuildingSys.TargetBuildingType.kind);
            buildingNumberLabel.text = totalNum.ToString();

            buildingTypeDescLabel.text = buildBuildingSys.TargetBuildingType.desc;
            buildingTypeItem.SetSelected(true);
        }

        public void OnPersonChange(List<Person> personList)
        {
            buildBuildingSys.personList = personList;

            buildBuildingSys.UpdateJobValue();
            buildCountLabel.text = $"{buildBuildingSys.wonderBuildCounter}回";

            for (int i = 0; i < personItems.Length; ++i)
            {
                if (i < personList.Count)
                    personItems[i].SetPerson(personList[i]);

                else
                    personItems[i].SetPerson(null);
            }
        }

        public void OnSelectPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(buildBuildingSys.TargetCity.freePersons,
                buildBuildingSys.personList, 3, OnPersonChange, buildBuildingSys.customTitleList, buildBuildingSys.customTitleName);
        }

        /// <summary>
        /// 新建建筑
        /// </summary>
        public void OnBuildBuilding()
        {
            buildBuildingSys.DoBuildBuilding();
            OnShow();
        }

        /// <summary>
        /// 升级建筑
        /// </summary>
        public void OnUpgradeBuilding()
        {
            buildBuildingSys.DoUpgradeBuilding();
            OnShow();
        }

        /// <summary>
        /// 拆除建筑
        /// </summary>
        public void OnDestroyBuilding()
        {
            buildBuildingSys.DoDestroyBuilding();
            OnShow();
        }
    }
}
