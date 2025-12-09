
using Sango.Game.Render.Model;
using Sango.Game.Render.UI;
using Sango.Loader;
using Sango.Render;
using UnityEngine;

namespace Sango.Game.Render
{
    public class BuildingBaseRender : ObjectRender
    {
        BuildingBase Building { get; set; }
        BuildingBaseModel BuildingModel { get; set; }
        UGUIWindow HeadBar { get; set; }

        bool isComplate = false;
        public BuildingBaseRender(BuildingBase building)
        {
            Owener = building;
            Building = building;
            MapObject = MapObject.Create($"{Building.BelongCity?.Name}-{Building.Name}", "Building");
            MapObject.objType = Building.BuildingType.kind;
            MapObject.modelId = Building.BuildingType.Id;

            isComplate = building.isComplte;
            if (!string.IsNullOrEmpty(building.model))
                MapObject.modelAsset = building.model;
            else
            {
                if (building.isComplte)
                    MapObject.modelAsset = building.BuildingType.model;
                else
                    MapObject.modelAsset = building.BuildingType.modelBroken;
            }

            MapObject.transform.position = Building.CenterCell.Position + new Vector3(0, building.heightOffset, 0);
            MapObject.transform.rotation = Quaternion.Euler(new Vector3(0, Building.rot * Mathf.Rad2Deg, 0));
            MapObject.transform.localScale = Vector3.one;
            MapObject.bounds = new Sango.Tools.Rect(0, 0, 32, 32);
            MapObject.onModelLoadedCallback = OnModelLoaded;
            MapObject.onModelVisibleChange = OnModelVisibleChange;
            MapRender.Instance.AddStatic(MapObject);
            UpdateInfo();
        }

        protected virtual string GetHeadbarAsset()
        {
            return null;
        }

        public void OnModelLoaded(GameObject obj)
        {
            BuildingModel = MapObject.GetComponentInChildren<BuildingBaseModel>(true);
            if (BuildingModel != null)
            {
                BuildingModel.Init(Building);
            }

            if (HeadBar != null)
            {
                PoolManager.Recycle(HeadBar.gameObject);
                HeadBar = null;
            }

            string headbarAsset = GetHeadbarAsset();
            GameObject headBar = PoolManager.Create(headbarAsset);
            if (headBar != null)
            {
                headBar.transform.SetParent(obj.transform, false);
                headBar.transform.localPosition = Vector3.zero;
                BillboardUI billboardUI = headBar.GetComponent<BillboardUI>();
                if (billboardUI != null)
                {
                    //billboardUI.cacheOffset = new Vector3(0, 30, 0);
                    billboardUI.Update();
                }
                UGUIWindow uGUIWindow = headBar.GetComponent<UGUIWindow>();
                if (uGUIWindow != null)
                {
                    UIBuildingBaseHeadbar uITroopHeadbar = uGUIWindow as UIBuildingBaseHeadbar;
                    if (uITroopHeadbar != null)
                    {
                        uITroopHeadbar.Init(Building);
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
                UIBuildingBaseHeadbar uITroopHeadbar = HeadBar as UIBuildingBaseHeadbar;
                if (uITroopHeadbar != null)
                {
                    uITroopHeadbar.UpdateState(Building);
                }
            }

            if (BuildingModel != null)
            {
                BuildingModel.Init(Building);
            }

            if (isComplate == false && Building.isComplte)
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

        public override void ShowInfo(int damage, int damageType)
        {
            if (HeadBar != null)
            {
                UIBuildingBaseHeadbar uITroopHeadbar = HeadBar as UIBuildingBaseHeadbar;
                if (uITroopHeadbar != null)
                {
                    uITroopHeadbar.ShowInfo(damage, damageType);
                }
            }
        }
    }
}
