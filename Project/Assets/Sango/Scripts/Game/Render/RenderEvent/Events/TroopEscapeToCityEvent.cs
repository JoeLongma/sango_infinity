using Sango.Render;
using System;
using UnityEngine;


namespace Sango.Game.Render
{
    public class TroopEscapeToCityEvent : RenderEventBase
    {
        public Troop troop;
        public City dest;
        public System.Action doneAction;

        public override void Enter(Scenario scenario)
        {
            
        }

        public override void Exit(Scenario scenario)
        {
            
        }

        public override bool IsVisible()
        {
            return troop.Render.IsVisible();
        }

        public override bool Update(Scenario scenario, float deltaTime)
        {
            if (troop.TryMoveToCity(dest))
            {
                // 移动完成，进入城市
                if (troop.cell.building == dest)
                {
                    troop.EnterCity(dest);
                }
                return true;
            }

            return false;
        }
    }
}
