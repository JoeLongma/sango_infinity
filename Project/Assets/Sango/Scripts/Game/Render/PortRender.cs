using LuaInterface;
using Sango.Game.Render.Model;
using Sango.Game.Render.UI;
using Sango.Loader;
using Sango.Render;
using UnityEngine;

namespace Sango.Game.Render
{
    public class PortRender : BuildingBaseRender
    {
        public PortRender(Port city) : base(city)
        {

        }

        protected override string GetHeadbarAsset()
        {
            return GameRenderHelper.CityHeadbarRes;
        }
    }
}
