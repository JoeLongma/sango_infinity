namespace Sango.Game.Render
{
    public class TroopSpellSkillEvent : RenderEventBase
    {
        public Troop troop;
        public Skill skill;
        public Cell spellCell;
        private bool isAction = false;
        private float time = 0;
        public override void Enter(Scenario scenario)
        {
            isAction = false;
            time = 0;
            if (IsVisible())
            {
                troop.Render.SetSmokeShow(true);
            }
            if (skill.costEnergy > 0)
                troop.Render.ShowSkill(skill, false, false);
        }

        public override void Exit(Scenario scenario)
        {
            if (IsVisible())
            {
                troop.Render.SetSmokeShow(false);
            }
        }

        public override bool IsVisible()
        {
            return troop.Render.IsVisible();
        }

        public override bool Update(Scenario scenario, float deltaTime)
        {
            if (!IsVisible())
            {
                Action();
                IsDone = true;
                return IsDone;
            }
            IsDone = skill.UpdateRender(troop, spellCell, scenario, time, Action);
            time += deltaTime;
            return IsDone;
        }


        public void Action()
        {
            if (isAction) return;
            skill.Action(troop, spellCell, 100);
            isAction = true;
        }

    }
}
