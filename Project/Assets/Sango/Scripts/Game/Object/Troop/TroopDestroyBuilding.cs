using System;

namespace Sango.Game
{
    public class TroopDestroyBuilding : TroopMissionBehaviour
    {
        public override MissionType MissionType { get { return MissionType.TroopDestroyBuilding; } }

        public override bool IsMissionComplete => throw new NotImplementedException();

        public override bool DoAI(Troop troop, Scenario scenario)
        {
            return true;
        }
    }
}
