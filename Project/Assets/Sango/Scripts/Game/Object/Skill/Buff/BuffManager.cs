using Newtonsoft.Json;
using Sango.Game.Render;
using Sango.Render;
using System.Collections.Generic;
using UnityEngine;

namespace Sango.Game
{
    [JsonObject(MemberSerialization.OptIn)]

    public class BuffManager
    {
        [JsonProperty]
        public List<BuffInstance> _buffs = new List<BuffInstance>();

        public class BuffEffectInfo
        {
            public string name;
            public int refCount;
            public Vector3 offset;
            public GameObject instanceObj;

            public void CreateAsset(SangoObject target)
            {
                if (instanceObj != null)
                    return;

                ObjectRender objectRender = target.GetRender();
                if (!objectRender.IsVisible()) return;

                instanceObj = PoolManager.Create(name);
                if (instanceObj != null)
                {
                    instanceObj.transform.SetParent(objectRender.GetTransform(), false);
                    instanceObj.transform.localPosition = offset;
                }
            }

            public void ClearAsset()
            {
                if (instanceObj != null)
                {
                    PoolManager.Recycle(instanceObj);
                    instanceObj = null;
                }
            }

        }

        Dictionary<string, BuffEffectInfo> assetRef = new Dictionary<string, BuffEffectInfo>();

        public Troop Master { get; private set; }

        public void Init(Troop master)
        {
            Master = master;
            foreach (BuffInstance ins in _buffs)
                ins.Init(this, ins.Buff, ins.Master);
        }

        public void OnModelLoaded(GameObject model)
        {
            foreach (BuffEffectInfo info in assetRef.Values)
                info.CreateAsset(Master);
        }

        public void OnModelClear()
        {
            foreach (BuffEffectInfo info in assetRef.Values)
                info.ClearAsset();
        }


        public void AddBuff(int id)
        {
            Buff buff = Scenario.Cur.GetObject<Buff>(id);
        }

        public void RemoveBuff(int id)
        {

        }

        public void OnForceTurnStart(Scenario scenario)
        {
            for (int i = 0; i < _buffs.Count; i++)
            {
                BuffInstance buff = _buffs[i];
                buff.TurnUpdate();
            }

            _buffs.RemoveAll(x => x.leftCounter <= 0);
        }

        public bool HasControlState()
        {
            return false;
        }

        public void CreateAsset(string asset, Vector3 offset)
        {
            if (assetRef.TryGetValue(asset, out BuffEffectInfo refInfo))
            {
                refInfo.refCount++;
                return;
            }
            BuffEffectInfo buffEffectInfo = new BuffEffectInfo()
            {
                name = asset,
                offset = offset,
                refCount = 1
            };
            assetRef[asset] = buffEffectInfo;

            buffEffectInfo.CreateAsset(Master);
        }

        public void ReleaseAsset(string asset)
        {

        }
    }
}
