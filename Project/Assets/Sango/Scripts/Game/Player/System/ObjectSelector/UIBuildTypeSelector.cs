using Sango.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIBuildTypeSelector : UGUIWindow
    {
        List<BuildingType> BuildingTypes { get; set; }
        public UIBuildingTypeItem[] buildingTypeItems;
        public GameObject buildButtonObj;
        public UIPersonItem[] personItems;

        public UITextField buildCountLabel;
        public UITextField cityGoldLabel;
        public UITextField buildingNumberLabel;
        public UITextField developNumberLabel;
        public UITextField emptyNumberLabel;
        public UITextField buildingTypeDescLabel;
        public UITextField limitLabel;
        public UITextField durabilityLabel;

        CityBuildBuilding buildBuildingSys;
        int maxPage;
        int curPage;
        int lastSelectIndex = -1;
        public Text pageLabel;

        public override void OnShow()
        {


            buildCountLabel.text = "";
            cityGoldLabel.text = "";
            buildCountLabel.text = "";
            buildButtonObj.SetActive(false);
            buildBuildingSys = Singleton<CityBuildBuilding>.Instance;
            BuildingTypes = buildBuildingSys.canBuildBuildingType;
            curPage = 0;
            maxPage = BuildingTypes.Count / buildingTypeItems.Length;
            if (BuildingTypes.Count % buildingTypeItems.Length != 0)
                maxPage += 1;

            for (int i = 0; i < buildingTypeItems.Length; i++)
            {
                UIBuildingTypeItem item = buildingTypeItems[i];
                item.onSelected = OnSelectBuildingType;
            }
            ShowPage(curPage);
            buildBuildingSys.SelectBuildingType(BuildingTypes[0]);
            UpdateContent();
        }

        public void ShowPage(int index)
        {
            if (index < 0 || index >= maxPage)
                return;
            curPage = index;

            pageLabel.text = $"{curPage + 1}/{maxPage}";

            for (int i = 0; i < buildingTypeItems.Length; i++)
            {
                UIBuildingTypeItem item = buildingTypeItems[i];
                int id = curPage * buildingTypeItems.Length + i;
                if (id < BuildingTypes.Count)
                {
                    BuildingType buildingType = BuildingTypes[id];
                    item.gameObject.SetActive(true);
                    item.SetBuildingType(buildingType).SetIndex(id).SetSelected(buildingType == buildBuildingSys.TargetBuildingType).SetNum(-1);
                }
                else
                {
                    item.gameObject.SetActive(false);
                }
            }
        }

        public void NextPage()
        {
            ShowPage(curPage + 1);
        }

        public void PrevPage()
        {
            ShowPage(curPage - 1);
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void OnCancel()
        {
            buildBuildingSys.Done();
        }

        public void OnSelectBuildingType(UIBuildingTypeItem buildingTypeItem)
        {
            if (lastSelectIndex == -1)
                lastSelectIndex = buildingTypeItem.index;
            else
            {
                if (lastSelectIndex >= curPage * buildingTypeItems.Length)
                {
                    int idx = lastSelectIndex - curPage * buildingTypeItems.Length;
                    buildingTypeItems[idx].SetSelected(false);
                }
            }

            lastSelectIndex = buildingTypeItem.index;
            buildingTypeItem.SetSelected(true);
            BuildingType targetBuildingType = BuildingTypes[buildingTypeItem.index];
            buildButtonObj.SetActive(true);
            buildBuildingSys.SelectBuildingType(targetBuildingType);

            buildCountLabel.text = $"{buildBuildingSys.wonderBuildCounter}回";
            cityGoldLabel.text = $"{targetBuildingType.cost}/{buildBuildingSys.TargetCity.gold}";

            buildingTypeDescLabel.text = targetBuildingType.desc;
            limitLabel.text = targetBuildingType.limitDesc;
            buildingTypeItem.SetSelected(true);

            UpdateContent();

        }

        public void UpdateContent()
        {
            for (int i = 0; i < personItems.Length; ++i)
            {
                if (i < buildBuildingSys.personList.Count)
                    personItems[i].SetPerson(buildBuildingSys.personList[i]);
                else
                    personItems[i].SetPerson(null);
            }
        }

        /// <summary>
        /// 新建建筑
        /// </summary>
        public void OnBuild()
        {
            buildBuildingSys.OnSelectCell();
            Hide();
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
    }
}
