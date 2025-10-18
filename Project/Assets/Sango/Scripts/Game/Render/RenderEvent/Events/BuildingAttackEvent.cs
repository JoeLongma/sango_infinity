using Sango.Render;
using UnityEngine;


namespace Sango.Game.Render
{
    public class BuildingAttackEvent : RenderEventBase
    {
        public Building building;
        public Cell targetCell;
        private bool isAction = false;
        private float time = 0;
        public override void Enter(Scenario scenario)
        {
            isAction = false;
            time = 0;
            if (IsVisible())
            {
                //troop.Render.SetSmokeShow(true);
            }
        }

        public override void Exit(Scenario scenario)
        {
            if (IsVisible())
            {
                //troop.Render.SetSmokeShow(false);
            }
        }

        public override bool IsVisible()
        {
            return building.Render.IsVisible();
        }

        public override bool Update(Scenario scenario, float deltaTime)
        {
            if (!IsVisible())
            {
                Action();
                IsDone = true;
                return IsDone;
            }

            if (time <= 0f)
            {
                //troop.Render.FaceTo(targetCell.Position);
                //troop.Render.SetAniShow(1);
            }
            if (time > 0.5f)
            {
                Action();
                IsDone = true;
            }
            time += deltaTime;
            return IsDone;
        }


        public void Action()
        {
            if (isAction) return;

            Troop troop = targetCell.troop;
            if (troop != null && building.IsEnemy(troop))
            {
                int dmg = Troop.CalculateSkillDamage(building, troop, building.GetAttack());
                troop.ChangeTroops(-dmg, building);
#if SANGO_DEBUG
                Sango.Log.Print($"{troop.BelongForce.Name}的[{troop.Name} - {troop.TroopType.Name}] 受到 {building.BelongForce?.Name}的[{building.Name}]伤害:{dmg}, 目标剩余兵力: {troop.GetTroopsNum()}");
#endif
                troop.Render.UpdateRender();
            }
            isAction = true;
        }

    }
}
