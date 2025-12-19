using Sango.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityCreateItems : UICityComandBase
    {
        public UIBuildingTypeItem objUIBuildingTypeItem;
        List<UIBuildingTypeItem> buildingTypeItemPool = new List<UIBuildingTypeItem>();

        public Text infoLabel;
        public Text itemTypeDescLabel;

        public Scrollbar scrollbar;
        public VerticalLayoutGroup rootLayout;
        CityCreateItems createItemsSys;
        int updateNextFrame = 0;
        public override void OnInit()
        {
            base.OnInit();
            createItemsSys = currentSystem as CityCreateItems;
            title1?.gameObject.SetActive(true);
            value_1?.gameObject.SetActive(true);
            title_value?.gameObject.SetActive(true);
            value_value?.gameObject.SetActive(true);
            title_gold?.gameObject.SetActive(true);
            value_gold?.gameObject.SetActive(true);
            infoLabel.text = "兵装";

            int len = createItemsSys.ItemTypes.Count;
            while (buildingTypeItemPool.Count < len)
            {
                GameObject go = GameObject.Instantiate(objUIBuildingTypeItem.gameObject, objUIBuildingTypeItem.transform.parent);
                UIBuildingTypeItem buildingTypeItem = go.GetComponent<UIBuildingTypeItem>();
                buildingTypeItemPool.Add(buildingTypeItem);
                buildingTypeItem.onSelected = OnSelectItemType;
                go.SetActive(true);
            }

            for (int i = len; i < buildingTypeItemPool.Count; i++)
                buildingTypeItemPool[i].gameObject.SetActive(false);

            for (int i = 0; i < len; i++)
            {
                CityCreateItems.ItemTypeInfo itemType = createItemsSys.ItemTypes[i];
                int totalNum = createItemsSys.TargetCity.itemStore.GetNumber(itemType.itemType.Id);
                UIBuildingTypeItem cityBuildingSlot = buildingTypeItemPool[i];
                cityBuildingSlot.SetItemType(itemType.itemType).SetIndex(i).SetSelected(itemType == createItemsSys.CurSelectedItemType).SetNum(totalNum);
                cityBuildingSlot.SetValid(itemType.targetBuilding != null);
                buildingTypeItemPool[i].gameObject.SetActive(true);
            }
            action_value.text = $"{JobType.GetJobCostAP((int)CityJobType.CreateItems)}/{createItemsSys.TargetCity.BelongCorps.ActionPoint}";

            OnSelectItemType(buildingTypeItemPool[createItemsSys.CurSelectedItemTypeIndex]);
            updateNextFrame = 2;
        }

        private void LateUpdate()
        {
            if (updateNextFrame > 0)
            {
                updateNextFrame--;
                if(updateNextFrame == 0)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rootLayout.GetComponent<RectTransform>());
                }
            }
        }

        public void OnSelectItemType(UIBuildingTypeItem buildingTypeItem)
        {
            if (createItemsSys.CurSelectedItemTypeIndex >= 0)
            {
                buildingTypeItemPool[createItemsSys.CurSelectedItemTypeIndex].SetSelected(false);
            }

            createItemsSys.CurSelectedItemTypeIndex = buildingTypeItem.index;
            CityCreateItems.ItemTypeInfo curItemType = createItemsSys.ItemTypes[buildingTypeItem.index];
            createItemsSys.CurSelectedItemType = curItemType;

            Person[] builder = ForceAI.CounsellorRecommendCreateItems(createItemsSys.TargetCity.freePersons);
            createItemsSys.personList.Clear();
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
                        createItemsSys.personList.Add(person);
                    }
                    else
                    {
                        personItems[i].SetPerson(null);
                    }
                }
            }

            createItemsSys.UpdateJobValue();

            value_1.text = $"{createItemsSys.TurnAndDestNumber[0]}回";
            value_gold.text = $"{curItemType.itemType.cost}/{createItemsSys.TargetCity.gold}";

            int totalNum = createItemsSys.TargetCity.itemStore.GetNumber(curItemType.itemType);
            value_value.text = $"{totalNum}→{totalNum + createItemsSys.TurnAndDestNumber[1]}";

            itemTypeDescLabel.text = curItemType.itemType.desc;
            buildingTypeItem.SetSelected(true);
        }

        public void OnPersonChange(List<Person> personList)
        {
            createItemsSys.personList = personList;
            createItemsSys.UpdateJobValue();

            CityCreateItems.ItemTypeInfo curItemType = createItemsSys.CurSelectedItemType;

            value_1.text = $"{createItemsSys.TurnAndDestNumber[0]}回";
            value_gold.text = $"{curItemType.itemType.cost}/{createItemsSys.TargetCity.gold}";

            int totalNum = createItemsSys.TargetCity.itemStore.GetNumber(curItemType.itemType);
            value_value.text = $"{totalNum}→{totalNum + createItemsSys.TurnAndDestNumber[1]}";

            for (int i = 0; i < personItems.Length; ++i)
            {
                if (i < createItemsSys.personList.Count)
                    personItems[i].SetPerson(createItemsSys.personList[i]);

                else
                    personItems[i].SetPerson(null);
            }
        }

        public void OnSelectPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(createItemsSys.TargetCity.freePersons,
                createItemsSys.personList, 3, OnPersonChange, createItemsSys.customTitleList, createItemsSys.customTitleName);
        }

        public void OnDo()
        {
            createItemsSys.DoJob();
            OnShow();
        }
    }
}
