using Sango.Render;
using UnityEngine;

namespace Sango.Game.Render
{
    public class TroopRender : ObjectRender
    {
        Troop Troop { get; set; }
        UnityEngine.UI.Text textInfo { get; set; }
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
            if (obj.visible == false) return;

            TroopsRender troopsRender = obj.GetComponentInChildren<TroopsRender>(true);
            if (troopsRender != null)
            {

            }


            FlagRender flagRender = obj.GetComponentInChildren<FlagRender>(true);
            if (flagRender != null)
            {
                flagRender.Init(Troop);
            }
        }

        public void UpdateInfo()
        {
            textInfo.color = Troop.BelongForce.Flag.color;
            textInfo.text = $"<{Troop.BelongForce.Name}>\n[{Troop.Name}队 - {Troop.TroopType.Name}]\n [{Troop.troops}] \n -{Troop.food}-";
        }

        public override void UpdateRender()
        {
            base.UpdateRender();
            UpdateInfo();
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}
