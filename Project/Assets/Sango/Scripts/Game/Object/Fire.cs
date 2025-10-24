using Newtonsoft.Json;
using Sango.Game.Render;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Fire : SangoObject
    {
        [JsonProperty]
        public int intelligence;

        [JsonProperty]
        public int counter;

        /// <summary>
        /// 所在格子
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(XY2CellConverter))]
        public Cell cell;

        public FireRender Render { get; private set; }

        public override void Init(Scenario scenario)
        {
            Render = new FireRender(this);
        }

        public override void OnScenarioPrepare(Scenario scenario)
        {

        }

        public override bool OnTurnStart(Scenario scenario)
        {
            ActionOver = false;

            return true;
        }

        public override bool OnTurnEnd(Scenario scenario)
        {
            if (!ActionOver)
            {
                Action();
            }
            return true;
        }

        public void Action()
        {

            ActionOver = true;
        }

    }
}
