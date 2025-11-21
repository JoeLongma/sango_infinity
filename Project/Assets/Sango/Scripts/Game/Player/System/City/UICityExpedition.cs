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

        public override void OnShow()
        {
            //buildBuildingSys = Singleton<CityBuildBuilding>.Instance;
            //buildBuildingSys.CurSelectSlotIndex = -1;
            //int slotLength = buildBuildingSys.TargetCity.innerSlot.Length;
            //while (buildingSlotPool.Count < slotLength)
            //{
            //    GameObject go = GameObject.Instantiate(objUICityBuildingSlot.gameObject, objUICityBuildingSlot.transform.parent);
            //    UICityBuildingSlot cityBuildingSlot = go.GetComponent<UICityBuildingSlot>();
            //    buildingSlotPool.Add(cityBuildingSlot);
            //    cityBuildingSlot.onSelected = OnSelectSlot;
            //    go.SetActive(true);
            //}

            //for (int i = slotLength; i < buildingSlotPool.Count; i++)
            //    buildingSlotPool[i].gameObject.SetActive(false);

            //for (int i = 0; i < slotLength; i++)
            //{
            //    int buildingId = buildBuildingSys.TargetCity.innerSlot[i];
            //    UICityBuildingSlot cityBuildingSlot = buildingSlotPool[i];
            //    if (buildingId > 0)
            //    {
            //        Building building = Scenario.Cur.GetObject<Building>(buildingId);
            //        cityBuildingSlot.SetBuilding(building).SetIndex(i).SetSelected(false);
            //    }
            //    else
            //    {
            //        if (buildBuildingSys.CurSelectSlotIndex < 0)
            //            buildBuildingSys.CurSelectSlotIndex = i;
            //        cityBuildingSlot.SetBuilding(null).SetIndex(i).SetSelected(false);
            //    }
            //}

            //OnSelectSlot(buildingSlotPool[buildBuildingSys.CurSelectSlotIndex]);
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void OnCancel()
        {
            cityExpeditionSys.Done();
        }

    
        //public void ShowBuildingType()
        //{
        //    int len = buildBuildingSys.canSelectBuildingTypes.Count;
        //    while (buildingTypeItemPool.Count < len)
        //    {
        //        GameObject go = GameObject.Instantiate(objUIBuildingTypeItem.gameObject, objUIBuildingTypeItem.transform.parent);
        //        UIBuildingTypeItem buildingTypeItem = go.GetComponent<UIBuildingTypeItem>();
        //        buildingTypeItemPool.Add(buildingTypeItem);
        //        buildingTypeItem.onSelected = OnSelectBuildingType;
        //        go.SetActive(true);
        //    }

        //    for (int i = len; i < buildingTypeItemPool.Count; i++)
        //        buildingTypeItemPool[i].gameObject.SetActive(false);

        //    for (int i = 0; i < len; i++)
        //    {
        //        BuildingType buildingType = buildBuildingSys.canSelectBuildingTypes[i];

        //        if (buildBuildingSys.CurSelectBuildingTypeIndex < 0)
        //            buildBuildingSys.CurSelectBuildingTypeIndex = i;


        //        int totalNum = buildBuildingSys.TargetCity.GetIntriorBuildingComplateNumber(buildingType.kind);

        //        UIBuildingTypeItem cityBuildingSlot = buildingTypeItemPool[i];
        //        cityBuildingSlot.SetBuildingType(buildingType).SetIndex(i).SetSelected(false).SetNum(totalNum);
        //    }

        //    OnSelectBuildingType(buildingTypeItemPool[buildBuildingSys.CurSelectBuildingTypeIndex]);
        //}

        //public void OnSelectBuildingType(UIBuildingTypeItem buildingTypeItem)
        //{
        //    if (buildBuildingSys.CurSelectBuildingTypeIndex >= 0)
        //    {
        //        buildingTypeItemPool[buildBuildingSys.CurSelectBuildingTypeIndex].SetSelected(false);
        //    }

        //    buildBuildingSys.CurSelectBuildingTypeIndex = buildingTypeItem.index;
        //    buildBuildingSys.TargetBuildingType = buildBuildingSys.canSelectBuildingTypes[buildingTypeItem.index];

        //    Person[] builder = ForceAI.CounsellorRecommendBuild(buildBuildingSys.TargetCity.freePersons, buildBuildingSys.TargetBuildingType);
        //    buildBuildingSys.personList.Clear();
        //    if (builder == null || builder.Length == 0)
        //    {
        //        for (int i = 0; i < personItems.Length; ++i)
        //            personItems[i].SetPerson(null);
        //    }
        //    else
        //    {
        //        for (int i = 0; i < personItems.Length; ++i)
        //        {
        //            if (i < builder.Length)
        //            {
        //                Person person = builder[i];
        //                personItems[i].SetPerson(person);
        //                buildBuildingSys.personList.Add(person);
        //            }
        //            else
        //            {
        //                personItems[i].SetPerson(null);
        //            }
        //        }
        //    }
        //    buildBuildingSys.UpdateJobValue();
        //    buildCountLabel.text = $"{buildBuildingSys.wonderBuildCounter}回";
        //    cityGoldLabel.text = $"{buildBuildingSys.TargetBuildingType.cost}/{buildBuildingSys.TargetCity.gold}";

        //    int totalNum = buildBuildingSys.TargetCity.GetIntriorBuildingComplateNumber(buildBuildingSys.TargetBuildingType.kind);
        //    buildingNumberLabel.text = totalNum.ToString();

        //    buildingTypeDescLabel.text = buildBuildingSys.TargetBuildingType.desc;
        //    buildingTypeItem.SetSelected(true);
        //}

        public void OnPersonChange(List<Person> personList)
        {
            //buildBuildingSys.UpdateJobValue();
            //buildCountLabel.text = $"{buildBuildingSys.wonderBuildCounter}回";

            //for (int i = 0; i < personItems.Length; ++i)
            //{
            //    if (i < buildBuildingSys.personList.Count)
            //        personItems[i].SetPerson(buildBuildingSys.personList[i]);

            //    else
            //        personItems[i].SetPerson(null);
            //}
        }

        public void OnSelectPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(cityExpeditionSys.TargetCity.freePersons,
                cityExpeditionSys.personList, 3, OnPersonChange, cityExpeditionSys.customTitleList, cityExpeditionSys.customTitleName);
        }
    }
}
