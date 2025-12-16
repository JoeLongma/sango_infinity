using Sango.Game.Render.UI;

namespace Sango.Game.Player
{
    public class PlayerTurnStartGreeting : CommandSystemBase
    {
        public PlayerTurnStartGreeting()
        {

        }

        public override void OnEnter()
        {
            Force force = Scenario.Cur.CurRunForce;
            UIDialog dialog = UIDialog.Open(UIDialog.DialogStyle.ClickPersonSay, $"<color=#00ffff>{force}</color>大人，\n终于轮到我们了啊。", () => { Done(); });
            Person person = force.Counsellor;
            if (person != null || person.BelongForce != force)
            {
                int max = force.Governor.BelongCity.allPersons.Count;
                person = force.Governor.BelongCity.allPersons.Get(GameRandom.Range(0, max));
            }
            dialog.SetPerson(person);
            dialog.cancelAction = () =>
             {
                 Done();
             };
        }

        public override void OnDestroy()
        {
            UIDialog.Close();
        }

        public override void HandleEvent(CommandEventType eventType, Cell cell, UnityEngine.Vector3 clickPosition, bool isOverUI)
        {
            switch (eventType)
            {
                case CommandEventType.Cancel:
                case CommandEventType.ClickUp:
                case CommandEventType.RClickUp:
                    PlayerCommand.Instance.Back(); break;
            }

        }
    }
}
