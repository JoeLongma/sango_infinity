using Newtonsoft.Json;
using Sango.Game.Render;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Port : City
    {
        public override void OnPrepareRender()
        {
            Render = new PortRender(this);
        }

        public override bool OnForceTurnStart(Scenario scenario)
        {
            return base.OnForceTurnStart(scenario);
        }
    }
}
