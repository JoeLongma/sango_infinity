using Newtonsoft.Json.Utilities.LinqBridge;
using Sango.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIBuildingUpgrade : UGUIWindow
    {
        public Text windiwTitle;

        public UIPersonItem[] personItems;

        public UITextField value_turn;
        public UITextField value_gold;
        public UITextField action_value;

        BuildingActionUpgrade buildingActionUpgrade;


        public override void OnShow()
        {
            buildingActionUpgrade = Singleton<BuildingActionUpgrade>.Instance;

            for (int i = 0; i < personItems.Length; ++i)
            {
                if (i < buildingActionUpgrade.personList.Count)
                {
                    personItems[i].SetPerson(buildingActionUpgrade.personList[i]);
                }
                else
                {
                    personItems[i].SetPerson(null);
                }
            }

            value_turn.text = $"{buildingActionUpgrade.wonderBuildCounter * 10}日";
            value_gold.text = $"{buildingActionUpgrade.TargetUpgradeType.cost}/{buildingActionUpgrade.TargetBuilding.BelongCity.gold}";

        }


        public void OnSure()
        {
            buildingActionUpgrade.DoJob();
        }

        public void OnCancel()
        {
            buildingActionUpgrade.Exit();
        }
        public virtual void OnSelectPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(buildingActionUpgrade.TargetBuilding.BelongCity.freePersons,
               buildingActionUpgrade.personList, 3, OnPersonChange, buildingActionUpgrade.customTitleList, buildingActionUpgrade.customTitleName);

        }

        public virtual void OnPersonChange(List<Person> personList)
        {
            buildingActionUpgrade.personList = personList;
            buildingActionUpgrade.UpdateJobValue();
            OnShow();
        }

    }
}
