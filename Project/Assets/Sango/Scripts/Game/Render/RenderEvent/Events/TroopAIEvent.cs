using Sango.Render;
using System;
using UnityEngine;


namespace Sango.Game.Render
{
    public class TroopAIEvent : RenderEventBase
    {
        public Troop troop;
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
            if (!troop.IsAlive)
            {
                IsDone = true;
                doneAction?.Invoke();
                return IsDone;
            }

            IsDone = troop.DoAI(scenario);
            return IsDone;
        }
    }
}
