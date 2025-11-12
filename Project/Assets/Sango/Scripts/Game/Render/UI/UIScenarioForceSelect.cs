using Sango.Loader;
using Sango.Render;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIScenarioForceSelect : UGUIWindow
    {
        public Image forceHead;
        public Text forceInfo;
        public Text forceDesc;


        public GameObject cityObject;

        public void Awake()
        {
            Scenario scenario = Scenario.CurSelected;

            for (int i = 0; i < scenario.citySet.Count; i++)
            {
                

            }
        }

    }
}
