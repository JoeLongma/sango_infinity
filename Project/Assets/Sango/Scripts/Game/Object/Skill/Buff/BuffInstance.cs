using Newtonsoft.Json;
using System.Collections.Generic;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]

    public class BuffInstance
    {
        public BuffManager Manager { get; private set; }

        [JsonProperty]
        [JsonConverter(typeof(Id2ObjConverter<Troop>))]
        public Troop Master { get; private set; }

        [JsonProperty]
        [JsonConverter(typeof(Id2ObjConverter<Buff>))]
        public Buff Buff { get; private set; }

        [JsonProperty]
        public int leftCounter;

        public List<Action.ActionBase> actions = new List<Action.ActionBase>();

        public void Init(BuffManager manager, Buff buff, Troop master)
        {
            Manager = manager;
            Master = master;
            Buff = buff;
            Manager.CreateAsset(Buff.asset, Buff.offset);
            Buff.InitActions(actions, Manager.Master, Master, Buff);
        }

        public bool TurnUpdate()
        {
            leftCounter--;
            if (leftCounter <= 0)
            {
                Clear();
                return true;
            }
            return false;
        }

        public void Clear()
        {
            Manager.ReleaseAsset(Buff.asset);
            if (actions != null)
            {
                for (int i = 0; i < actions.Count; i++)
                    actions[i].Clear();

                actions.Clear();
                actions = null;
            }
        }
    }
}
