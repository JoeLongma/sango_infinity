using Sango.Game.Player;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityRecruit : UGUIWindow
    {
        public Text windiwTitle;

        public UIPersonItem targetPersonItems;
        public UIStatusItem targetStatusItem;
        public UITextField targetForce;
        public UITextField targetLoyalty;
        public UITextField targetDays;
        public UITextField targetCityName;
        public UIStatusItem actionStatusItem;
        public UIPersonItem actionPersonItems;

        public UITextField action_value;

        City TargetCity;
        CityRecruit currentSystem;
        public Button sureButton;


        public override void OnShow()
        {
            currentSystem = Singleton<CityRecruit>.Instance;
            TargetCity = currentSystem.TargetCity;
            UpdateContent();
        }

        public void UpdateContent()
        {
            Person target = currentSystem.target.Count > 0 ? currentSystem.target[0] : null;
            Person action = currentSystem.personList.Count > 0 ? currentSystem.personList[0] : null;
            action_value.text = $"{JobType.GetJobCostAP((int)CityJobType.RecuritPerson)}/{TargetCity.BelongCorps.ActionPoint}";
            sureButton.interactable = target != null && action != null;

            targetPersonItems.SetPerson(target);
            actionPersonItems.SetPerson(action);
            targetStatusItem.SetPerson(target);
            actionStatusItem.SetPerson(action);

            if (target != null)
            {
                targetForce.text = target.BelongForce != null ? target.BelongForce.Name : "--";
                targetLoyalty.text = target.loyalty.ToString();
                targetDays.text = $"{target.BelongCity.Distance(TargetCity)}日";
                targetCityName.text = target.BelongCity.Name;
            }
            else
            {
                targetForce.text = "--";
                targetLoyalty.text = "--";
                targetDays.text = "--";
                targetCityName.text = "--";
            }

        }

        public void OnSure()
        {
            currentSystem.DoJob();
        }

        public void OnCancel()
        {
            currentSystem.Exit();
        }

        public void OnSelectTargetPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(currentSystem.targetList,
               currentSystem.target, 1, OnTargetPersonChange, currentSystem.customTargetTitleList, currentSystem.customTargetTitleName);

        }

        public void OnSelectActionPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(TargetCity.freePersons,
               currentSystem.personList, 1, OnActionPersonChange, currentSystem.customActionTitleList, currentSystem.customActionTitleName);

        }

        public virtual void OnTargetPersonChange(List<Person> personList)
        {
            currentSystem.SetTarget(personList);
            UpdateContent();
        }

        public virtual void OnActionPersonChange(List<Person> personList)
        {
            currentSystem.personList = personList;
            UpdateContent();
        }

    }
}
