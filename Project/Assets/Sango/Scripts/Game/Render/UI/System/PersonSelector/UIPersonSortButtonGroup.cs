using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game.Render.UI
{
    public class UIPersonSortButtonGroup : MonoBehaviour
    {
        List<UIPersonSortButton> groupList = new List<UIPersonSortButton>();
        public void Add(UIPersonSortButton button)
        {
            groupList.Remove(button);
            groupList.Add(button);
        }

        public void Select(UIPersonSortButton button)
        {
            foreach (UIPersonSortButton other in groupList)
                if (other != button)
                    other.Clear();
        }

    }
}