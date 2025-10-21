using Newtonsoft.Json;
using Sango.Game.Render;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Gate : City
    {
        public override void OnPrepareRender()
        {
            Render = new GateRender(this);
        }

    }
}
