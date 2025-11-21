using Newtonsoft.Json.Utilities.LinqBridge;
using Sango.Game.Player;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityComandBase : UGUIWindow
    {
        public Text windiwTitle;

        public UIPersonItem[] personItems;
        protected CityComandBase currentSystem;

        public UITextField title1;
        public UITextField value_1;

        public UITextField title_value;
        public UITextField value_value;

        public UITextField title_gold;
        public UITextField value_gold;

        public UITextField title_extra;
        public UITextField value_extra;

        public Button button_extra;
        public Text button_extra_title;
        public UITextField button_extra_value;

        public UITextField person_extra_value;

        public Action initAction;

        public void Init(CityComandBase cityComandBase)
        {
            currentSystem = cityComandBase;
            title1.gameObject.SetActive(false);
            value_1.gameObject.SetActive(false);
            title_value.gameObject.SetActive(false);
            value_value.gameObject.SetActive(false);
            title_gold.gameObject.SetActive(false);
            value_gold.gameObject.SetActive(false);
            title_extra.gameObject.SetActive(false);
            value_extra.gameObject.SetActive(false);
            button_extra.gameObject.SetActive(false);
            button_extra_value.gameObject.SetActive(false);
            person_extra_value.gameObject.SetActive(false);
            OnInit();
        }

        public void Init(CityComandBase cityComandBase, Action uiAction)
        {
            initAction = uiAction;
            Init(cityComandBase);
        }

        public virtual void OnInit()
        {
            for (int i = 0; i < personItems.Length; ++i)
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
            initAction?.Invoke();
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
            currentSystem.OnSelectPerson();
        }

        public void OnSelectCity()
        {
            currentSystem.OnSelectCity();
        }
    }
}
