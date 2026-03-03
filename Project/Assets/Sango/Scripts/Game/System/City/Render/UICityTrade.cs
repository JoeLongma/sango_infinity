using Sango.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityTrade : UGUIWindow
    {
        public Text windiwTitle;

        public UIPersonItem targetPersonItems;
        public UIStatusItem targetStatusItem;
        public UITextField targetEffectValue;
        public UITextField targetGold;
        public UITextField targetFood;
        public UITextField targetPercent;
        public Slider tradeSlider;

        public UITextField action_value;

        City TargetCity;
        CityTrade currentSystem;
        public Button sureButton;

        public override void OnShow()
        {
            currentSystem = Singleton<CityTrade>.Instance;
            TargetCity = currentSystem.TargetCity;
            UpdateContent();
        }

        public void UpdateContent()
        {
            int count = currentSystem.personList.Count;
            Person target = count > 0 ? currentSystem.personList[0] : null;
            action_value.text = $"{count * JobType.GetJobCostAP((int)CityJobType.TradeFood)}/{TargetCity.BelongCorps.ActionPoint}";
            sureButton.interactable = target != null;
            targetPersonItems.SetPerson(target);
            targetStatusItem.SetPerson(target);
            targetGold.text = $"{(count * JobType.GetJobCost((int)CityJobType.TradeFood))}/{TargetCity.gold}";
        }

        public void OnSure()
        {
            currentSystem.DoJob();
        }

        public void OnCancel()
        {
            currentSystem.Exit();
        }

        public void OnSelectPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(currentSystem.TargetCity.freePersons,
               currentSystem.personList, 1, OnPersonChange, currentSystem.customTitleList, currentSystem.customTitleName);
        }

        public virtual void OnPersonChange(List<Person> personList)
        {
            currentSystem.personList = (personList);
            currentSystem.UpdateJobValue();
            UpdateContent();
        }

        public void OnSelectFood()
        {

        }

        public void OnTradeSliderValueChanged(float p)
        {
            
        }
    }
}
