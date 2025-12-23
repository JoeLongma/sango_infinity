using Sango.Game.Player;
using Sango.Loader;
using Sango.Render;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIObjectPopInfo : UGUIWindow
    {
        public UIMiniBuldingInfoPanel buildingInfoPanel;
        public UIMiniCityInfoPanel cityInfoPanel;
        public UIMiniPortInfoPanel portInfoPanel;
        public UIMiniTroopInfoPanel troopInfoPanel;

        UIMiniInfoPanel currentPanel;
        SangoObject currentObject;

        public override void OnShow()
        {
            base.OnShow();
            ResetAllPanel(null);
            currentPanel = null;
            currentObject = null;
            GameController.Instance.onCellOverEnter += OnCellOverEnter;
            GameController.Instance.onCellOverExit += OnCellOverExit;
        }

        void ResetAllPanel(UIMiniInfoPanel except)
        {
            if (except != buildingInfoPanel && buildingInfoPanel.gameObject.activeSelf)
                buildingInfoPanel.gameObject.SetActive(false);
            if (except != cityInfoPanel && cityInfoPanel.gameObject.activeSelf)
                cityInfoPanel.gameObject.SetActive(false);
            if (except != portInfoPanel && portInfoPanel.gameObject.activeSelf)
                portInfoPanel.gameObject.SetActive(false);
            if (except != troopInfoPanel && troopInfoPanel.gameObject.activeSelf)
                troopInfoPanel.gameObject.SetActive(false);
        }

        public override void OnHide()
        {
            GameController.Instance.onCellOverEnter -= OnCellOverEnter;
            GameController.Instance.onCellOverExit -= OnCellOverExit;
            base.OnHide();
        }

        void OnCellOverEnter(Cell cell)
        {
            if (cell == null)
            {
                if (currentPanel != null)
                {
                    currentPanel.gameObject.SetActive(false);
                    currentPanel = null;
                    currentObject = null;
                }
                return;
            }

            Vector2 screenPos = Camera.main.WorldToScreenPoint(cell.Position);

            Sango.Log.Error($"screenPos : {screenPos}");
            Vector2 anchorPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(),
                screenPos, Game.Instance.UICamera, out anchorPos);

            Sango.Log.Error($"anchorPos : {anchorPos}");

            if (cell.building != null)
            {
                if (currentObject == cell.building)
                    return;
                currentObject = cell.building;
                if (cell.building.IsCity())
                {
                    ResetAllPanel(cityInfoPanel);
                    cityInfoPanel.Show(cell.building as City);
                    currentPanel = cityInfoPanel;
                }
                else if (cell.building.IsCityBase())
                {
                    ResetAllPanel(portInfoPanel); 
                    portInfoPanel.Show(cell.building as City);
                    currentPanel = portInfoPanel;
                }
                else
                {
                    ResetAllPanel(buildingInfoPanel);
                    buildingInfoPanel.Show(cell.building as Building);
                    currentPanel = buildingInfoPanel;
                }
            }
            else if (cell.troop != null)
            {
                if (currentObject == cell.troop)
                    return;
                currentObject = cell.building;
                ResetAllPanel(troopInfoPanel);
                troopInfoPanel.Show(cell.troop);
                currentPanel = troopInfoPanel;
            }
            else
            {
                currentPanel = null;
                ResetAllPanel(null);
                currentObject = null;
            }

            if (currentPanel == null)
            {
                return;
            }

            if (!currentPanel.gameObject.activeSelf)
                currentPanel.gameObject.SetActive(true);

            bool showUp = screenPos.y < Screen.height / 2;
            bool showLeft = screenPos.x <= (Screen.width / 2);
            Sango.Log.Error($"showUp : {showUp}");
            Sango.Log.Error($"showLeft : {showLeft}");
            if (showUp)
            {
                anchorPos.y += (currentPanel.root.sizeDelta.y + 10);
            }
            else
            {
                anchorPos.y -= 10;
            }

            if (showLeft)
            {
                anchorPos.x += (currentPanel.root.sizeDelta.x + 10);
            }
            else
            {
                anchorPos.x -= 10;

            }

            currentPanel.root.anchoredPosition = anchorPos;

        }

        void OnCellOverExit(Cell cell)
        {

        }

    }
}
