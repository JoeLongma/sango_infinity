using LuaInterface;
using Sango.Loader;
using Sango.Render;
using UnityEngine;

namespace Sango.Game.Render
{
    public class TroopRender : ObjectRender
    {
        Troop Troop { get; set; }
        UnityEngine.UI.Text textInfo { get; set; }

        UGUIWindow HeadBar { get; set; }
        string headbarKey = "troops_head_bar";

        public TroopRender(Troop troop)
        {
            Owener = troop;
            Troop = troop;

            MapObject = MapObject.Create(Troop.Name + "队");
            MapObject.objType = Troop.TroopType.Id;
            MapObject.modelId = Troop.TroopType.Id;
            MapObject.modelAsset = Troop.TroopType.model;
            MapObject.transform.position = troop.cell.Position;
            MapObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            MapObject.transform.localScale = Vector3.one;
            MapObject.bounds = new Sango.Tools.Rect(0, 0, 32, 32);
            MapObject.modelLoadedCallback = OnModelLoaded;
            MapObject.onModelVisibleChange = OnModelVisibleChange;
            MapRender.Instance.AddDynamic(MapObject);

            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>("TroopName")) as GameObject;
            obj.transform.SetParent(MapObject.transform, false);
            obj.transform.localPosition = new Vector3(0, 20, 0);
            UnityEngine.UI.Text text = obj.GetComponent<UnityEngine.UI.Text>();
            textInfo = text;
            UpdateInfo();
        }

        void OnModelLoaded(GameObject obj)
        {
            //TroopsRender troopsRender = obj.GetComponent<TroopsRender>();
            //if(troopsRender != null )
            //{

            //}

            //FlagRender flagRender = obj.GetComponent<FlagRender>();
            //if (flagRender != null)
            //{
            //    flagRender.Init(Troop);
            //}
        }

        void OnModelVisibleChange(MapObject obj)
        {
            if (obj.visible == false)
            {
                if(HeadBar != null)
                {
                    PoolManager.Recycle(headbarKey, HeadBar.gameObject);
                    HeadBar = null;
                }
                return;
            }
            TroopsRender troopsRender = obj.GetComponentInChildren<TroopsRender>(true);
            if (troopsRender != null)
            {
                troopsRender.SetShowPercent(Troop.troops / 10000.0f);
            }

            FlagRender flagRender = obj.GetComponentInChildren<FlagRender>(true);
            if (flagRender != null)
            {
                flagRender.Init(Troop);
            } 

            GameObject headBar = PoolManager.Get(headbarKey);
            if (headBar == null)
            {
                GameObject headBarObj = ObjectLoader.LoadObject<GameObject>("Assets/UI/Prefab/window_troop_bar.prefab");
                if(headBarObj != null)
                {
                    PoolManager.Add(headbarKey, headBarObj);
                    headBar = PoolManager.Get(headbarKey);
                }
            }

            if(headBar != null)
            {
                headBar.transform.SetParent(obj.transform, false);
                headBar.transform.localPosition = new Vector3(0, 25, 0);
                UGUIWindow uGUIWindow = headBar.GetComponent<UGUIWindow>();
                if(uGUIWindow != null)
                {
                    string windowName = "window_troop_bar";
                    LuaTable table = Window.Instance.FindPeerTable(new Window.WindowInfo()
                    {
                        name = windowName,
                        packageName = null,
                        resName = null,
                        scriptName = null
                    });

                    if (table != null)
                    {
                        uGUIWindow.AttachScript(table, false);
                        uGUIWindow.CallFunction("Awake", Troop);
                    }
                }
                HeadBar = uGUIWindow;
            }
        }

        public void UpdateInfo()
        {
            textInfo.color = Troop.BelongForce.Flag.color;
            textInfo.text = $"<{Troop.BelongForce.Name}>\n[{Troop.Name}队 - {Troop.TroopType.Name}]\n [{Troop.troops}] \n -{Troop.food}-";

            if(HeadBar != null)
            {
                HeadBar.CallFunction("UpdateState", Troop);

                TroopsRender troopsRender = MapObject.GetComponentInChildren<TroopsRender>(true);
                if (troopsRender != null)
                {
                    troopsRender.SetShowPercent(Troop.troops / 10000.0f);
                }
            }
        }

        public override void UpdateRender()
        {
            base.UpdateRender();
            UpdateInfo();
        }

        public override void Clear()
        {
            if (HeadBar != null)
            {
                PoolManager.Recycle(headbarKey, HeadBar.gameObject);
                HeadBar = null;
            }

            base.Clear();
        }
    }
}
