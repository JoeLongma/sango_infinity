using Sango.Render;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sango.Game
{
    public class GameController : Singletion<GameController>
    {
        public delegate void OnClickCell(Cell cell);
        public delegate void OnDoubleClickCell(Cell cell);

        public delegate void OnMouseOverEnter(Cell cell);
        public delegate void OnMouseOverExit(Cell cell);

        public delegate void OnCancel();
        public delegate void OnKeyDown(KeyCode keyCode);

        public OnClickCell onClickCell;
        public OnDoubleClickCell onDoubleClickCell;
        public OnCancel onCancel;
        public OnKeyDown onKeyDown;



        public void OnClick()
        {

        }

        public void OnSelectMapObject(MapObject mapObject, Vector3 selectPoint)
        {
            Sango.Log.Error(mapObject.gameObject.name);
        }

        public void OnSelectTerrain(Vector3 point)
        {

        }

    }

}
