using Sango.Render;
using System.Collections.Generic;
using UnityEngine;


namespace Sango.Game.Render
{
    public class FireDamageEvent : RenderEventBase
    {
        public Fire fire;
        public int damage;
        public Troop targetTroop;
        public BuildingBase targetBuilding;
        public float actTime = 0.1f;
        private float time = 0;
        public override void Enter(Scenario scenario)
        {
            time = 0;
            if(targetTroop != null)
            {
                targetTroop.ChangeTroops(-damage, fire, null, 0);
            }
            else if(targetBuilding != null)
            {
                targetBuilding.ChangeDurability(-damage, fire);
            }
        }

        public override void Exit(Scenario scenario)
        {
           
        }

        public override bool IsVisible()
        {
            if (targetTroop != null && targetTroop.IsAlive)
                return targetTroop.Render.IsVisible();

            if (targetBuilding != null && targetBuilding.IsAlive)
                return targetBuilding.Render.IsVisible();

            return false;
        }

        public override bool Update(Scenario scenario, float deltaTime)
        {
            if (!IsVisible())
            {
                IsDone = true;
                return IsDone;
            }

            time += deltaTime;

            if (time < actTime)
                return false;

            //if (targetTroop != null)
            //{
            //    targetTroop.Render.ShowInfo(damage, (int)InfoTyoe.Troop);
            //}

            //if (targetBuilding != null)
            //{
            //    targetBuilding.Render.ShowInfo(damage, (int)InfoTyoe.Durability);
            //}

            IsDone = true;
            return IsDone;
        }
    }
}
