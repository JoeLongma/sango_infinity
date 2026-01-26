using Newtonsoft.Json.Utilities.LinqBridge;
using Sango.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIBuildingWorkSet : UGUIWindow
    {
        public Text windiwTitle;

        public UIPersonItem[] personItems;

        public UITextField value_target;
        public UITextField value_attr;
        public UITextField value_product;
        public UITextField value_factor;

        public UITextField action_value;

        BuildingWorking buildingWorking;
        Building TargetBuilding { get; set; }

        List<Person> selectedPersonList = new List<Person>();

        public override void OnShow()
        {
            buildingWorking = Singleton<BuildingWorking>.Instance;
            TargetBuilding = buildingWorking.TargetBuilding;
            BuildingType targetBuildingType = TargetBuilding.BuildingType;
            selectedPersonList.Clear();
            if (TargetBuilding.Workers != null)
                TargetBuilding.Workers.ForEach(p =>
                {
                    selectedPersonList.Add(p);
                });


            for (int i = 0; i < personItems.Length; ++i)
            {
                UIPersonItem personItem = personItems[i];
                personItem.gameObject.SetActive(i < targetBuildingType.workerLimit);
                if (i < selectedPersonList.Count)
                {
                    personItems[i].SetPerson(selectedPersonList[i]);
                }
                else
                {
                    personItems[i].SetPerson(null);
                }
            }

            value_target.text = TargetBuilding.Name;
            value_attr.text = Scenario.Cur.Variables.GetAttributeNameWithColor(targetBuildingType.effectAttrType);

        }


        public void OnSure()
        {
            buildingWorking.SetBuildingWorker(TargetBuilding, selectedPersonList);
            buildingWorking.Done();
        }

        public void OnCancel()
        {
            buildingWorking.Exit();
        }

        public void OnAuto()
        {
            buildingWorking.AutoSetWorker(TargetBuilding);
            OnShow();
        }

        public virtual void OnSelectPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(TargetBuilding.BelongCity.freePersons,
               selectedPersonList, 3, OnPersonChange, buildingWorking.customTitleList, buildingWorking.customTitleName);

        }

        public virtual void OnPersonChange(List<Person> personList)
        {
            selectedPersonList = personList;
            OnShow();
        }
    }
}
