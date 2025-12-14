using Sango.Game.Render.UI;

namespace Sango.Game.Player
{
    public class TroopInteractiveBase : CommandSystemBase
    {
        public Troop TargetTroop { get; set; }
        public Cell ActionCell { get; set; }

        public string content;

        public override void Init()
        {
            GameEvent.OnTroopInteractiveContextDialogShow += OnTroopInteractiveContextDialogShow;
        }

        public override void Clear()
        {
            GameEvent.OnTroopInteractiveContextDialogShow -= OnTroopInteractiveContextDialogShow;
        }

        protected virtual void OnTroopInteractiveContextDialogShow(TroopInteractiveDialogData dialogData, Troop troop, Cell actionCell)
        {
           
            if (troop.BelongForce != null && troop.BelongForce.IsPlayer && troop.BelongForce == Scenario.Cur.CurRunForce)
            {
                TargetTroop = troop;
                ActionCell = actionCell;

                if (Check(troop, actionCell))
                {
                    dialogData.content = content;
                    dialogData.sureAction = OnSure;
                }
            }
        }

        protected virtual bool Check(Troop troop, Cell actionCell)
        {
            return false;
        }

        protected virtual void OnSure()
        {
            Start(TargetTroop, ActionCell);
        }

        public virtual void Start(Troop troop, Cell actionCell)
        {
            TargetTroop = troop;
            ActionCell = actionCell;
            PlayerCommand.Instance.Push(this);
        }
    }
}
