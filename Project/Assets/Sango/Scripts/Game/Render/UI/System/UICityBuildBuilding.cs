using Sango.Game.Player;
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

        List<BuildingType> canSelectBuildingTypes = new List<BuildingType>();
        public UIBuildingTypeItem objUIBuildingTypeItem;
        List<UIBuildingTypeItem> buildingTypeItemPool = new List<UIBuildingTypeItem>();



        public UIPersonItem[] personItems;
        public Text cityCurTroopsLabel;
        public Text cityDestTroopsLabel;
        public Text cityGoldLabel;
        CityBuildBuilding buildBuildingSys;
        public override void OnShow()
        {
            buildBuildingSys = CityBuildBuilding.Instance;
            int slotLength = buildBuildingSys.TargetCity.innerSlot.Length;
            if (buildingSlotPool.Count < slotLength)
            {
                GameObject go = GameObject.Instantiate(objUICityBuildingSlot.gameObject, objUICityBuildingSlot.transform.parent);
                UICityBuildingSlot cityBuildingSlot = go.GetComponent<UICityBuildingSlot>();
                buildingSlotPool.Add(cityBuildingSlot);
                cityBuildingSlot.onSelected = OnSelectSlot;
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
                    cityBuildingSlot.SetBuilding(building).SetIndex(i);
                }
                else
                {
                    cityBuildingSlot.SetBuilding(null).SetIndex(i);
                }
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void OnCancel()
        {
            buildBuildingSys.Exit();
        }

        public void OnSelectSlot(UICityBuildingSlot slot)
        {
            canSelectBuildingTypes.Clear();
            // 根据建筑情况,展示其他信息
            int buildingId = buildBuildingSys.TargetCity.innerSlot[slot.index];
            if (buildingId > 0)
            {
                Building building = Scenario.Cur.GetObject<Building>(buildingId);
                upgradeButtonObj.SetActive(!building.isUpgrading && building.BuildingType.nextId != 0);

                if (building.BuildingType.nextId != 0)
                {
                    BuildingType nextBuildingType = Scenario.Cur.GetObject<BuildingType>(building.BuildingType.nextId);
                    canSelectBuildingTypes.Add(nextBuildingType);
                    ShowBuildingType();
                }
            }
            else
            {
                canSelectBuildingTypes.Clear();
                for (int i = (int)BuildingKindType.Farm; i < (int)BuildingKindType.ArrowTower; ++i)
                {
                    canSelectBuildingTypes.Add(Scenario.Cur.GetObject<BuildingType>(i));
                }
                ShowBuildingType();
            }
        }

        public void ShowBuildingType()
        {
            int len = canSelectBuildingTypes.Count;
            if (buildingTypeItemPool.Count < len)
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
                BuildingType buildingType = canSelectBuildingTypes[i];
                UIBuildingTypeItem cityBuildingSlot = buildingTypeItemPool[i];
                cityBuildingSlot.SetBuildingType(buildingType);
            }
        }

        public void OnSelectBuildingType(UIBuildingTypeItem buildingTypeItem)
        {

        }

        public void OnPersonChange(List<Person> personList)
        {
            buildBuildingSys.UpdateJobValue();
            OnShow();
        }

        public void OnSelectPerson()
        {
            PersonSelectSystem.Instance.Start(buildBuildingSys.TargetCity.freePersons,
                buildBuildingSys.personList, 3, OnPersonChange, buildBuildingSys.customTitleList, buildBuildingSys.customTitleName);
        }

        /// <summary>
        /// 新建建筑
        /// </summary>
        public void OnBuildBuilding()
        {
            buildBuildingSys.DoJob();

        }

        /// <summary>
        /// 升级建筑
        /// </summary>
        public void OnUpgradeBuilding()
        {
            buildBuildingSys.DoJob();

        }

        /// <summary>
        /// 拆除建筑
        /// </summary>
        public void OnDestroyBuilding()
        {
            buildBuildingSys.DoJob();

        }
    }
}
