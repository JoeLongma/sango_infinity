using Sango.Game.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    /// <summary>
    /// 剧本选择界面
    /// </summary>
    public class UITechnique : UGUIWindow
    {
        public UITechniqueItem techniqueItem;
        List<UITechniqueItem> techniqueItemList = new List<UITechniqueItem>();
        public RectTransform techNode;
        public RectTransform personNode;
        public ScrollRect scrollRect;

        public Button sureBtn;

        public UITextField techDesc;
        public UITextField techCount;
        public UITextField techCost;
        public UITextField techCostTP;
        public UITextField techNeedAttr;
        public UITextField actionPointValue;

        public UIPersonItem[] personItems;
        public int itemWidth = 280;
        public int itemHeight = 80;
        public int linwWidth = 80;
        public int linhHeight = 80;

        public int woffset = 20;
        public int hoffset = 20;

        int maxCol = -99;
        int maxRow = -99;
        Force targetForce;
        City targetCity;
        CityTechniqueResearch techniqueResearchSys;
        UITechniqueItem selectedItem;
        Technique selectTech;

        void InitItems()
        {
            if (techniqueItemList.Count > 0)
                return;
            Scenario.Cur.CommonData.Techniques.ForEach(technique =>
            {
                AddTechniqueItem(technique);
            });

            for (int i = 0; i < techniqueItemList.Count; i++)
            {
                CompleteTheLine(techniqueItemList[i]);
            }

            RectTransform scroll = scrollRect.GetComponent<RectTransform>();
            scrollRect.horizontal = techNode.sizeDelta.x > scroll.sizeDelta.x;
            scrollRect.vertical = techNode.sizeDelta.y > scroll.sizeDelta.y;
        }

        public UITechniqueItem AddTechniqueItem(Technique technique)
        {
            UITechniqueItem techItem = GameObject.Instantiate(techniqueItem.gameObject, techniqueItem.transform.parent).GetComponent<UITechniqueItem>();
            techItem.gameObject.SetActive(true);
            techniqueItemList.Add(techItem);
            techItem.root.anchoredPosition = new Vector2(technique.col * itemWidth + woffset, -(technique.row * itemHeight) - hoffset);
            maxCol = Mathf.Max(maxCol, technique.col);
            maxRow = Mathf.Max(maxRow, technique.row);
            techNode.sizeDelta = new Vector2((maxCol + 1) * itemWidth + 2f * woffset, (maxRow + 1) * itemHeight + 2f * hoffset);
            techItem.SetTechnique(technique);
            techItem.onClick = OnSelectTechniqueItem;
            return techItem;
        }

        public void UpdateItems()
        {
            for (int i = 0; i < techniqueItemList.Count; i++)
            {
                UITechniqueItem techItem = techniqueItemList[i];
                Technique technique = techItem.technique;
                techItem.SetValid(technique.IsValid(targetForce)).SetCanResearch(technique.CanResearch(targetForce)).SetSelected(techItem == selectedItem);
            }
        }

        public void CompleteTheLine(UITechniqueItem techniqueItem)
        {
            Technique technique = techniqueItem.technique;
            if (technique.needTechs == null || technique.needTechs.Length == 0)
            {
                techniqueItem.lineNode.gameObject.SetActive(false);
                return;
            }

            techniqueItem.lineNode.gameObject.SetActive(true);
            Scenario scenario = Scenario.Cur;
            // 连线
            for (int i = 0; i < technique.needTechs.Length; i++)
            {
                Technique dep = scenario.GetObject<Technique>(technique.needTechs[i]);
                int colDis = technique.col - dep.col;
                int rowDis = technique.row - dep.row;

                int width = colDis * linwWidth;
                int height = rowDis * linhHeight;
                if (rowDis == 0)
                {
                    RectTransform linRect = GameObject.Instantiate(techniqueItem.lineMid.gameObject, techniqueItem.lineMid.transform.parent).GetComponent<RectTransform>();
                    linRect.gameObject.SetActive(true);
                    Vector2 size = linRect.sizeDelta;
                    size.x = width;
                    linRect.sizeDelta = size;
                }
                else if (rowDis > 0)
                {
                    RectTransform linRect = GameObject.Instantiate(techniqueItem.lineDown.gameObject, techniqueItem.lineDown.transform.parent).GetComponent<RectTransform>();
                    linRect.gameObject.SetActive(true);
                    Vector2 size = linRect.sizeDelta;
                    size.y = height;
                    linRect.sizeDelta = size;
                }
                else
                {
                    RectTransform linRect = GameObject.Instantiate(techniqueItem.lineUp.gameObject, techniqueItem.lineUp.transform.parent).GetComponent<RectTransform>();
                    linRect.gameObject.SetActive(true);
                    Vector2 size = linRect.sizeDelta;
                    size.y = height;
                    linRect.sizeDelta = size;
                }
            }
        }


        public override void OnShow()
        {
            sureBtn.interactable = false;
            selectedItem = null;
            techniqueResearchSys = Singleton<CityTechniqueResearch>.Instance;
            targetForce = techniqueResearchSys.TargetCity.BelongForce;
            targetCity = techniqueResearchSys.TargetCity;
            InitItems();
            UpdateItems();
            actionPointValue.text = $"{JobType.GetJobCostAP((int)CityJobType.Research)}/{techniqueResearchSys.TargetCity.BelongCorps.ActionPoint}";

        }

        public void OnSure()
        {
            techniqueResearchSys.DoResearch();

        }

        public void OnCancel()
        {
            techniqueResearchSys.Back();
        }

        public void OnSelectTechniqueItem(UITechniqueItem techniqueItem)
        {
            if (selectedItem == techniqueItem) return;
            if (selectedItem != null)
            {
                selectedItem.SetSelected(false);
            }
            techniqueItem.SetSelected(true);
            selectTech = techniqueItem.technique;

            techDesc.text = selectTech.desc;

            if (techniqueItem.CanResearch())
            {
                sureBtn.interactable = true;
                techniqueResearchSys.SelectTechnique(selectTech);
                techCount.text = $"{techniqueResearchSys.counter * 10}日";
                if (techniqueResearchSys.goldCost <= targetCity.gold)
                    techCost.text = $"{techniqueResearchSys.goldCost}/{targetCity.gold}";
                else
                {
                    techCost.text = $"<color=#ff1010>{techniqueResearchSys.goldCost}</color>/{targetCity.gold}";
                    sureBtn.interactable = false;
                }

                if (techniqueResearchSys.tpCost <= targetCity.BelongForce.TechniquePoint)
                    techCostTP.text = $"{techniqueResearchSys.tpCost}/{targetCity.BelongForce.TechniquePoint}";
                else
                {
                    techCostTP.text = $"<color=#ff1010>{techniqueResearchSys.tpCost}</color>/{targetCity.BelongForce.TechniquePoint}";
                    sureBtn.interactable = false;
                }
            }
            else if (techniqueItem.IsValid())
            {
                sureBtn.interactable = false;
                techCount.text = "--";
                techCost.text = "--";
                techCostTP.text = "--";
            }
            else
            {
                sureBtn.interactable = false;
                techCount.text = $"{selectTech.counter * 10}日";
                if (selectTech.goldCost <= targetCity.gold)
                    techCost.text = $"{selectTech.goldCost}/{targetCity.gold}";
                else
                    techCost.text = $"<color=#ff1010>{selectTech.goldCost}</color>/{targetCity.gold}";

                if (selectTech.techPointCost <= targetCity.BelongForce.TechniquePoint)
                    techCostTP.text = $"{selectTech.techPointCost }/{targetCity.BelongForce.TechniquePoint}";
                else
                    techCostTP.text = $"<color=#ff1010>{selectTech.techPointCost }</color>/{targetCity.BelongForce.TechniquePoint}";
            }
            techNeedAttr.text = Scenario.Cur.Variables.GetAttributeNameWithColor(selectTech.needAttr);

            for (int i = 0; i < personItems.Length; ++i)
            {
                if (i < techniqueResearchSys.personList.Count)
                    personItems[i].SetPerson(techniqueResearchSys.personList[i]);

                else
                    personItems[i].SetPerson(null);
            }
        }

        public void OnSelectPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(targetCity.freePersons,
                techniqueResearchSys.personList, 3, OnPersonChange, techniqueResearchSys.customTitleList, techniqueResearchSys.customTitleName);
        }

        public void OnPersonChange(List<Person> personList)
        {
            techniqueResearchSys.personList = personList;
            techniqueResearchSys.UpdateJobValue();

            //buildCountLabel.text = $"{buildBuildingSys.wonderBuildCounter}回";

            for (int i = 0; i < personItems.Length; ++i)
            {
                if (i < personList.Count)
                    personItems[i].SetPerson(personList[i]);

                else
                    personItems[i].SetPerson(null);
            }
        }
    }
}
