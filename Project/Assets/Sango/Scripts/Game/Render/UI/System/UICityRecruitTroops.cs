using Sango.Game.Player;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UICityRecruitTroops : UGUIWindow
    {
        public UIPersonItem[] personItems;
        public Text cityCurTroopsLabel;
        public Text cityDestTroopsLabel;
        public Text cityGoldLabel;
        CityRecruitTroops recruitTroopsSys;
        public override void OnShow()
        {
            recruitTroopsSys = CityRecruitTroops.Instance;
            for (int i = 0; i < 3; ++i)
            {
                if (i < recruitTroopsSys.personList.Count)
                {
                    personItems[i].SetPerson(recruitTroopsSys.personList[i]);
                }
                else
                {
                    personItems[i].SetPerson(null);
                }
            }

            cityCurTroopsLabel.text = recruitTroopsSys.TargetCity.troops.ToString();
            cityDestTroopsLabel.text = (recruitTroopsSys.TargetCity.troops + recruitTroopsSys.wonderTroopsAddNumber).ToString();
        }

        public void OnSure()
        {
            recruitTroopsSys.DoJob();

        }
        public void OnCancel()
        {
            recruitTroopsSys.Exit();
        }

        public void OnPersonChange(List<Person> personList)
        {
            recruitTroopsSys.UpdateJobValue();
            OnShow();
        }

        public void OnSelectPerson()
        {
            PersonSelectSystem.Instance.Start(recruitTroopsSys.TargetCity.freePersons,
                recruitTroopsSys.personList, 3, OnPersonChange);
        }
    }
}
