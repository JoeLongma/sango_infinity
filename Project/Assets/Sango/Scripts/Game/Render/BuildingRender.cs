using LuaInterface;
using Sango.Game.Render.Model;
using Sango.Game.Render.UI;
using Sango.Loader;
using Sango.Render;
using UnityEngine;

namespace Sango.Game.Render
{
    public class BuildingRender : ObjectRender
    {
        Building Building { get; set; }
        BuildingModel BuildingModel { get; set; }
        UGUIWindow HeadBar { get; set; }
        bool isComplate = false;
        public BuildingRender(Building building)
        {
            Owener = building;
            Building = building;
            MapObject = MapObject.Create($"{Building.BelongCity.Name}-{Building.Name}");
            MapObject.objType = Building.BuildingType.kind;
            MapObject.modelId = Building.BuildingType.Id;
            isComplate = building.isComplte;
            if (building.isComplte)
                MapObject.modelAsset = building.BuildingType.model;
            else
                MapObject.modelAsset = building.BuildingType.modelBroken;

            MapObject.transform.position = Building.CenterCell.Position;
            MapObject.transform.rotation = Quaternion.Euler(new Vector3(0, Building.rot, 0));
            MapObject.transform.localScale = Vector3.one;
            MapObject.bounds = new Sango.Tools.Rect(0, 0, 32, 32);
            MapObject.onModelLoadedCallback = OnModelLoaded;
            MapObject.onModelVisibleChange = OnModelVisibleChange;
            MapRender.Instance.AddStatic(MapObject);
            UpdateInfo();
        }

        public void OnModelLoaded(GameObject obj)
        {
            BuildingModel = MapObject.GetComponentInChildren<BuildingModel>(true);
            if (BuildingModel != null)
            {
                BuildingModel.Init(Building);
            }

            if (HeadBar != null)
            {
                PoolManager.Recycle(HeadBar.gameObject);
                HeadBar = null;
            }

            GameObject headBar = PoolManager.Create(GameRenderHelper.BuildingHeadbarRes);
            if (headBar != null)
            {
                headBar.transform.SetParent(obj.transform, false);
                headBar.transform.localPosition = new Vector3(0, 25, 0);

                UGUIWindow uGUIWindow = headBar.GetComponent<UGUIWindow>();
                if (uGUIWindow != null)
                {
                    string windowName = System.IO.Path.GetFileNameWithoutExtension(GameRenderHelper.BuildingHeadbarRes);
                    LuaTable table = Window.Instance.FindPeerTable(new Window.WindowInfo()
                    {
                        name = windowName,
                        packageName = null,
                        resName = null,
                        scriptName = null
                    });

                    if (table != null)
                    {
                        LuaFunction call = uGUIWindow.GetFunction("Create");
                        if (call != null)
                        {
                            LuaTable instance = call.Invoke<LuaTable>();
                            if (instance != null)
                            {
                                uGUIWindow.AttachScript(instance, true);
                                uGUIWindow.CallFunction("Init", Building);
                            }
                        }
                    }
                    else
                    {
                        UIBuildingHeadbar uITroopHeadbar = uGUIWindow as UIBuildingHeadbar;
                        if (uITroopHeadbar != null)
                        {
                            uITroopHeadbar.Init(Building);
                        }
                    }
                }
                HeadBar = uGUIWindow;
            }

        }

        void OnModelVisibleChange(MapObject obj)
        {
            if (obj.visible == false)
            {
                BuildingModel = null;
                if (HeadBar != null)
                {
                    PoolManager.Recycle(HeadBar.gameObject);
                    HeadBar = null;
                }
                return;
            }
        }

        public void UpdateInfo()
        {
            if (HeadBar != null)
            {
                if (HeadBar.HasScript())
                {
                    HeadBar.CallFunction("UpdateState", Building);
                }
                else
                {
                    UIBuildingHeadbar uITroopHeadbar = HeadBar as UIBuildingHeadbar;
                    if (uITroopHeadbar != null)
                    {
                        uITroopHeadbar.UpdateState(Building);
                    }
                }
            }

            //if (BuildingModel != null)
            //{
            //    BuildingModel.Init(Building);
            //}
            if(isComplate == false && Building.isComplte)
            {
                isComplate = Building.isComplte;
                MapObject.ChangeModel(Building.BuildingType.model);
            }
        }

        public override void UpdateRender()
        {
            base.UpdateRender();
            UpdateInfo();
        }

        public override void Clear()
        {
            BuildingModel = null;
            if (HeadBar != null)
            {
                PoolManager.Recycle(HeadBar.gameObject);
                HeadBar = null;
            }
            base.Clear();
        }

        public override void ShowDamage(int damage, int damageType)
        {
            if (HeadBar != null)
            {
                UIBuildingHeadbar uITroopHeadbar = HeadBar as UIBuildingHeadbar;
                if (uITroopHeadbar != null)
                {
                    uITroopHeadbar.ShowDamage(damage, damageType);
                }
            }
        }
    }
}
