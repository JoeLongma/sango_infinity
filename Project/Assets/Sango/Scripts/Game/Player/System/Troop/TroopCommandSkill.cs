using Sango.Game.Render;
using Sango.Game.Render.UI;
using System.Collections.Generic;

namespace Sango.Game.Player
{
    public class TroopCommandSkill : TroopComandBase
    {
        List<Cell> MovePath { get; set; }

        public TroopCommandSkill()
        {
            customMenuName = "战法";
            customMenuOrder = 0;
        }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            ContextMenu.CloseAll();

            MovePath = Singleton<TroopSystem>.Instance.movePath;
            Cell start = TargetTroop.cell;
            for (int i = 1; i < MovePath.Count; i++)
            {
                bool isLast = i == MovePath.Count - 1;
                Cell dest = MovePath[i];
                TroopMoveEvent @event = new TroopMoveEvent()
                {
                    troop = TargetTroop,
                    dest = dest,
                    start = start,
                    isLastMove = isLast
                };

                if (isLast)
                {
                    @event.doneAction = OnMoveDone;
                }

                RenderEvent.Instance.Add(@event);
                start = dest;
            }

        }

        public void OnMoveDone()
        {
            TargetTroop.ActionOver = true;
            Done();
        }
    }
}
