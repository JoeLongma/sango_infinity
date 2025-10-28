using Sango.Render;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sango.Game
{
    public class GameController : Singletion<GameController>
    {
        public bool Enabled { get; set; }
        enum ControlType : int
        {
            None = 0,
            Move,
            Rotate,
            Zoom,
            RotateAndZoom
        }
        ControlType controlType = ControlType.None;

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

        private Plane viewPlane = new Plane(Vector3.up, Vector3.zero);

        public void OnInputClick()
        {

        }

        public void OnInputCancel()
        {

        }

        public void OnInputMapObjectOverEnter(MapObject mapObject)
        {

        }
        public void OnInputMapObjectOverExit(MapObject mapObject)
        {

        }

        public void OnSelectMapObject(MapObject mapObject, Vector3 selectPoint)
        {
            Sango.Log.Error(mapObject.gameObject.name);
        }

        public void OnSelectTerrain(Vector3 point)
        {

        }

        bool IsOverUI()
        {
            return (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())/* || FairyGUI.Stage.isTouchOnUI*/;
        }
        bool IsOverUI(int fingerId)
        {
            return (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId))/* || FairyGUI.Stage.isTouchOnUI*/;
        }


        private MapObject mouseOverMapObject;
        private Vector3[] touchPos = new Vector3[2];
        private bool[] touchMoveFlag = new bool[2];
        private Vector3[] dragePos = new Vector3[2];
        private Ray ray;
        private RaycastHit hit;
        private int rayCastLayer = LayerMask.GetMask("Map", "Building", "Troops");
        public void Update()
        {
            if(!Enabled) return;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 2000, rayCastLayer))
            {
                MapObject mapObjcet = hit.collider.gameObject.GetComponentInParent<MapObject>();
                if (mapObjcet != mouseOverMapObject)
                {
                    if (mouseOverMapObject != null)
                        OnInputMapObjectOverExit(mouseOverMapObject);
                    if (mapObjcet != null)
                        OnInputMapObjectOverEnter(mapObjcet);
                    mouseOverMapObject = mapObjcet;
                }
            }
            else
            {
                mouseOverMapObject = null;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (IsOverUI()) return;

                if (mouseOverMapObject != null)
                    return;

                controlType = ControlType.Move;
                touchPos[0] = Input.mousePosition;
                float dis;
                if (viewPlane.Raycast(ray, out dis))
                    dragePos[0] = ray.GetPoint(dis);

            }
            else if (Input.GetMouseButtonDown(1))
            {
                controlType = ControlType.Rotate;
                touchPos[1] = Input.mousePosition;
            }
            else
            {
                float zoom = Input.GetAxis("Mouse ScrollWheel");
                if (zoom != 0)
                    controlType = ControlType.Zoom;
            }

            if (mouseOverMapObject != null && Input.GetMouseButtonUp(0))
            {
                OnSelectMapObject(mouseOverMapObject, hit.point);
                controlType = ControlType.None;
                return;
            }

#else
            for (int i = 0; i < Input.touchCount; i ++)
            {
                Touch touch = Input.GetTouch(i);
                if(touch.phase == TouchPhase.Began)
                {
                    if(Input.touchCount == 1)
                    {
                        touchPos[0] = touch.position;
                        controlType = ControlType.Move;
                        break;
                    }
                    else if(Input.touchCount == 2)
                    {
                        touchPos[0] = Input.GetTouch(0).position;
                        touchPos[0] = Input.GetTouch(1).position;
                        controlType = ControlType.RotateAndZoom;
                        break;
                    }
                }
            }

#endif

            switch (controlType)
            {
                case ControlType.Move:
                    OnInputMove();
                    break;
                case ControlType.Rotate:
                    OnInputRotate();
                    break;
                case ControlType.Zoom:
                    OnInputZoom();
                    break;
                case ControlType.RotateAndZoom:
                    OnInputZoomAndRotate();
                    break;
            }
        }

        public void OnInputMove()
        {
            if (Input.GetMouseButton(0))
            {
                if (touchPos[0] == Input.mousePosition)
                    return;

                touchMoveFlag[0] = true;
                Vector3 delta = Input.mousePosition - touchPos[0];
                touchPos[0] = Input.mousePosition;

                float dis;
                if (viewPlane.Raycast(ray, out dis))
                {
                    Vector3 newDragPos = ray.GetPoint(dis);
                    Vector3 offset = dragePos[0] - newDragPos;
                    MapRender.Instance.OffsetCamera(offset);
                }

            }
            else if (Input.GetMouseButtonUp(0))
            {
                controlType = ControlType.None;
                if (touchMoveFlag[0])
                {
                    touchMoveFlag[0] = false;
                    return;
                }

                if (Physics.Raycast(ray, out hit, 2000, rayCastLayer))
                {
                    MapObject mapObjcet = hit.collider.gameObject.GetComponentInParent<MapObject>();
                    if (mapObjcet != null)
                    {
                        OnSelectMapObject(mapObjcet, hit.point);
                    }
                    else
                    {
                        OnSelectTerrain(hit.point);
                    }
                }
            }
        }
        public void OnInputRotate()
        {
            if (Input.GetMouseButton(1))
            {
                if (touchPos[1] == Input.mousePosition)
                    return;

                touchMoveFlag[1] = true;
                Vector3 delta = Input.mousePosition - touchPos[1];
                touchPos[1] = Input.mousePosition;
                MapRender.Instance.RotateCamera(delta);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                controlType = ControlType.None;
                if (touchMoveFlag[1])
                {
                    touchMoveFlag[1] = false;
                    return;
                }
                OnInputCancel();
            }
        }
        public void OnInputZoom()
        {
            float offset = Input.GetAxis("Mouse ScrollWheel");
            MapRender.Instance.ZoomCamera(offset);
        }
        public void OnInputZoomAndRotate()
        {

        }

        private void MoveCameraKeyBoard()
        {
            //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))//(Input.GetAxis("Horizontal")<0)
            //{
            //    position += -transform.right * keyBoardMoveSpeed;
            //}
            //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            //{
            //    position += transform.right * keyBoardMoveSpeed;
            //}
            //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            //{
            //    Vector3 forward = transform.forward;
            //    forward.y = 0;
            //    forward.Normalize();
            //    position += forward * keyBoardMoveSpeed;
            //}
            //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            //{
            //    Vector3 forward = transform.forward;
            //    forward.y = 0;
            //    forward.Normalize();
            //    position += forward * -keyBoardMoveSpeed;
            //}
        }
    }

}
