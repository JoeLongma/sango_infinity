using Sango.Loader;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIMapCitySelectItem : MonoBehaviour
    {
        public GameObject selectedObj;
        public GameObject normalObj;
        public GameObject inavtiveObj;
        public Image[] colorImage;
        public City city;
        public Action<City, bool> onSelectAction;

        public void OnSelect()
        {
            selectedObj.SetActive(!selectedObj.activeSelf);
            onSelectAction?.Invoke(city, selectedObj.activeSelf);
        }

        public UIMapCitySelectItem SetInavtive(bool b)
        {
            inavtiveObj.SetActive(b);
            return this;
        }

        public UIMapCitySelectItem SetSelected(bool b)
        {
            selectedObj.SetActive(b);
            return this;
        }

        public UIMapCitySelectItem SetColor(Color c)
        {
            foreach (var item in colorImage)
                item.color = c;
            return this;
        }

    }
}