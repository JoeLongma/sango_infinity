using Sango.Game.Player;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityComandBase : UGUIWindow
    {
        public UIPersonItem[] personItems;
        protected CityComandBase currentSystem;
        public UITextField title1;
        public UITextField title_value;
        public UITextField title_gold;

        public UITextField value_1;
        public UITextField value_cur;
        public UITextField value_dest;
        public UITextField value_gold;

        public void Init(CityComandBase cityComandBase)
        {
            currentSystem = cityComandBase;
            OnInit();
        }

        public virtual void OnInit()
        {
            for (int i = 0; i < 3; ++i)
            {
                if (i < currentSystem.personList.Count)
                {
                    personItems[i].SetPerson(currentSystem.personList[i]);
                }
                else
                {
                    personItems[i].SetPerson(null);
                }
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

        public void OnPersonChange(List<Person> personList)
        {
            currentSystem.UpdateJobValue();
            OnInit();
        }

        public void OnSelectPerson()
        {
            Singleton<PersonSelectSystem>.Instance.Start(currentSystem.TargetCity.freePersons,
                currentSystem.personList, 3, OnPersonChange, currentSystem.customTitleList, currentSystem.customTitleName);
        }
    }
}
