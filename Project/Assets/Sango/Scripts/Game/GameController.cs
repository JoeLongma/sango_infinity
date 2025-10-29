using Sango.Game.Render.UI;
using Sango.Render;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sango.Game
{
    public class GameController : Singletion<GameController>
    {
        public bool Enabled { get; set; }
        enum ControlType : byte
        {
            None = 0,
            Tap,
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

        public GameController()
        {
            EventBase.OnContextMenuShow += OnContextMenuShow;
        }

        public void OnContextMenuShow(ContextMenuData contextMenuData)
        {
            if (contextMenuData.depth == -1)
            {
                contextMenuData.Add("开发");
                contextMenuData.Add("军事");
                contextMenuData.Add("政治");
                contextMenuData.AddLine();
                contextMenuData.Add("合批");
                contextMenuData.Add("发发发");
                contextMenuData.Add("啊啊啊啊");
            }
            else if (contextMenuData.depth == 0)
            {
                contextMenuData.Add("煩煩煩");
                contextMenuData.Add("哈哈哈");
            }
            else if (contextMenuData.depth == 1)
            {
                contextMenuData.Add("急急急");
                contextMenuData.Add("酷酷酷是");
            }
        }

        public void OnInputClick()
        {

        }

        public void OnInputCancel()
        {
            //Sango.Game.Render.UI.ContextMenu.Close();
        }

        public void OnInputMapObjectOverEnter(MapObject mapObject)
        {

        }
        public void OnInputMapObjectOverExit(MapObject mapObject)
        {

        }

        public void OnSelectMapObject(MapObject mapObject, Vector3 selectPoint)
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, selectPoint);
            Player.PlayerCommand.Instance.Push(new Player.ShowContextMenu()
            {
                screenPos = screenPos

            }); ;

            //Sango.Log.Error(mapObject.gameObject.name);
            //Sango.Game.Render.UI.ContextMenu.Close();
            //Sango.Game.Render.UI.ContextMenu.Show(screenPos);
        }

        public void OnSelectTerrain(Vector3 point)
        {

        }

        public bool IsOverUI()
        {
            return (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())/* || FairyGUI.Stage.isTouchOnUI*/;
        }
        public bool IsOverUI(int fingerId)
        {
            return (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId))/* || FairyGUI.Stage.isTouchOnUI*/;
        }

        private MapObject mouseOverMapObject;
        private Ray ray;
        private RaycastHit hit;
        private int rayCastLayer = LayerMask.GetMask("Map", "Building", "Troops");

        public MapObject CheckMouseIsOnMapObject(out Vector3 hitPiont)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 2000, rayCastLayer))
            {
                MapObject mapObjcet = hit.collider.gameObject.GetComponentInParent<MapObject>();
                if (mapObjcet != null)
                {
                    hitPiont = hit.point;
                    return mapObjcet;
                }
            }
            hitPiont = Vector3.zero;
            return null;
        }
       
        private Vector2[] touchPos = new Vector2[2];
        MapObject downSelectMapObject = null;
        bool isDragMoving = false;
        bool isRotateMoving = false;
        Vector3 rotatePosition;
        Vector3 dragPosition;

        Vector3 worldPlaneDragPosition;

        public void HandleWindowsEvent()
        {
            if (Input.GetMouseButton(0) && !isRotateMoving)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    dragPosition = Input.mousePosition;
                    OnDragWorldBegin();
                }
                else
                {
                    if (dragPosition != Input.mousePosition)
                    {
                        isDragMoving = true;
                        OnDragWorld();
                        dragPosition = Input.mousePosition;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0) && !isRotateMoving)
            {
                if (isDragMoving)
                {
                    isDragMoving = false;
                    OnDragWorldEnd();
                    return;
                }
                OnClickWorld();
            }
            else if (Input.GetMouseButton(1) && !isDragMoving)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    rotatePosition = Input.mousePosition;
                    OnRotateWorldBegin();
                }

                else
                {
                    if (rotatePosition != Input.mousePosition)
                    {
                        isRotateMoving = true;
                        OnRotateWorld();
                        rotatePosition = Input.mousePosition;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(1) && !isDragMoving)
            {
                if (isRotateMoving)
                {
                    isRotateMoving = false;
                    OnRotateWorldEnd();
                    return;
                }
                OnRClickWorld();
            }
            else
            {
                Vector2 scrollWheel = Input.mouseScrollDelta;
                if (scrollWheel.y != 0)
                {
                    OnScaleWorld(scrollWheel.y);
                }
            }
        }

        public void OnDragWorldBegin()
        {
            //Debug.LogError("OnDragWorldBegin");

            if (controlType != ControlType.None)
                return;

            if (IsOverUI()) return;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            if (mouseOverMapObject != null)
                return;
#endif
            downSelectMapObject = CheckMouseIsOnMapObject(out Vector3 hitPiont);
            if (downSelectMapObject != null)
                return;

            controlType = ControlType.Move;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float dis;
            if (viewPlane.Raycast(ray, out dis))
                worldPlaneDragPosition = ray.GetPoint(dis);
        }

        public void OnDragWorldEnd()
        {
            //Debug.LogError("OnDragWorldEnd");
            if (controlType != ControlType.Move)
                return;

            controlType = ControlType.None;
        }

        public void OnDragWorld()
        {
            //Debug.LogError("OnDragWorld");

            if (controlType != ControlType.Move)
                return;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float dis;
            if (viewPlane.Raycast(ray, out dis))
            {
                Vector3 newDragPos = ray.GetPoint(dis);
                Vector3 offset = worldPlaneDragPosition - newDragPos;
                MapRender.Instance.OffsetCamera(offset);
            }
        }

        public void OnClickWorld()
        {

        }

        public void OnRotateWorldBegin()
        {
            if (controlType != ControlType.None)
                return;

            if (IsOverUI()) return;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            if (mouseOverMapObject != null)
                return;
#endif

            downSelectMapObject = CheckMouseIsOnMapObject(out Vector3 hitPiont);
            if (downSelectMapObject != null)
                return;

            controlType = ControlType.Rotate;
        }

        public void OnRotateWorldEnd()
        {
            if (controlType != ControlType.Rotate)
                return;

            controlType = ControlType.None;
        }

        public void OnRotateWorld()
        {
            if (controlType != ControlType.Rotate)
                return;
            
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            Vector3 delta = Input.mousePosition - rotatePosition;
            MapRender.Instance.RotateCamera(delta);
#else
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            Vector2 touch1Dirction = touch1.position - touchPos[0];
            Vector2 touch2Dirction = touch2.position - touchPos[1];
            float dotAngle = Vector2.Dot(touch1Dirction, touch2Dirction);
            if (dotAngle > 0)
            {
                // rotate
                MapRender.Instance.RotateCamera(touch1.deltaPosition);
            }
            else
            {
                float len = (touch1.position - touch2.position).sqrMagnitude;
                float srcLen = (touchPos[0] - touchPos[1]).sqrMagnitude;
                float delta = Mathf.Max(Mathf.Abs(touch2.deltaPosition.x), Mathf.Abs(touch1.deltaPosition.x));
                if (len < srcLen)
                    delta = -delta;
                OnScaleWorld(delta / 500f);
            }
#endif
        }


        public void OnRClickWorld()
        {

        }

        public void OnScaleWorld(float delta)
        {
            MapRender.Instance.ZoomCamera(delta);
        }

        bool[] keyFlags = new bool[4];
        private bool MoveCameraKeyBoard()
        {
            Array.Clear(keyFlags, 0, 4);
            bool hasKey = false;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))//(Input.GetAxis("Horizontal")<0)
            {
                keyFlags[0] = true;
                hasKey = true;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                keyFlags[1] = true;
                hasKey = true;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                keyFlags[2] = true;
                hasKey = true;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                keyFlags[3] = true;
                hasKey = true;
            }

            if (hasKey)
            {
                MapRender.Instance.MoveCameraKeyBoard(keyFlags);
                if (controlType == ControlType.Move)
                    controlType = ControlType.None;
            }

            return hasKey;
        }




        public void Update()
        {
            if (!Enabled) return;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            HandleWindowsEvent();
            MoveCameraKeyBoard();
#else
            HandleMobileEvent();
#endif


            //            Player.PlayerCommand.Instance.Update();

            //            if (!Enabled) return;

            //#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            //            MoveCameraKeyBoard();

            //            if (lastMousePosition != Input.mousePosition)
            //            {
            //                lastMousePosition = Input.mousePosition;
            //                // 鼠标移动
            //                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //                if (Physics.Raycast(ray, out hit, 2000, rayCastLayer))
            //                {
            //                    MapObject mapObjcet = hit.collider.gameObject.GetComponentInParent<MapObject>();
            //                    if (mapObjcet != mouseOverMapObject)
            //                    {
            //                        if (mouseOverMapObject != null)
            //                            OnInputMapObjectOverExit(mouseOverMapObject);
            //                        if (mapObjcet != null)
            //                            OnInputMapObjectOverEnter(mapObjcet);
            //                        mouseOverMapObject = mapObjcet;
            //                    }
            //                }
            //                else
            //                {
            //                    mouseOverMapObject = null;
            //                }
            //            }

            //            if (Input.GetMouseButtonDown(0))
            //            {
            //                if (IsOverUI()) return;

            //                if (mouseOverMapObject != null)
            //                    return;

            //                controlType = ControlType.Move;
            //                touchPos[0] = Input.mousePosition;
            //                float dis;
            //                if (viewPlane.Raycast(ray, out dis))
            //                    dragePos[0] = ray.GetPoint(dis);

            //            }
            //            else if (Input.GetMouseButtonDown(1))
            //            {
            //                controlType = ControlType.Rotate;
            //                touchPos[1] = Input.mousePosition;
            //            }
            //            else
            //            {
            //                float zoom = Input.GetAxis("Mouse ScrollWheel");
            //                if (zoom != 0)
            //                    controlType = ControlType.Zoom;
            //            }

            //            if (mouseOverMapObject != null && Input.GetMouseButtonUp(0) && !touchMoveFlag[0])
            //            {
            //                OnSelectMapObject(mouseOverMapObject, hit.point);
            //                controlType = ControlType.None;
            //                return;
            //            }

            //#else
            //            for (int i = 0; i < Input.touchCount; i++)
            //            {
            //                Touch touch = Input.GetTouch(i);
            //                if (touch.phase == TouchPhase.Began)
            //                {
            //                    if (Input.touchCount == 1)
            //                    {
            //                        // 选存点击到的物体
            //                        ray = Camera.main.ScreenPointToRay(touch.position);
            //                        if (Physics.Raycast(ray, out hit, 2000, rayCastLayer))
            //                        {
            //                            MapObject mapObjcet = hit.collider.gameObject.GetComponentInParent<MapObject>();
            //                            if (mapObjcet != mouseOverMapObject)
            //                            {
            //                                if (mouseOverMapObject != null)
            //                                    OnInputMapObjectOverExit(mouseOverMapObject);
            //                                if (mapObjcet != null)
            //                                    OnInputMapObjectOverEnter(mapObjcet);
            //                                mouseOverMapObject = mapObjcet;
            //                                controlType = ControlType.None;
            //                                return;
            //                            }
            //                        }

            //                        touchPos[0] = touch.position;
            //                        controlType = ControlType.Move;

            //                        float dis;
            //                        if (viewPlane.Raycast(ray, out dis))
            //                            dragePos[0] = ray.GetPoint(dis);

            //                        break;
            //                    }
            //                    else if (Input.touchCount == 2)
            //                    {
            //                        touchPos[0] = Input.GetTouch(0).position;
            //                        touchPos[0] = Input.GetTouch(1).position;
            //                        controlType = ControlType.RotateAndZoom;
            //                        break;
            //                    }
            //                }
            //                else if (touch.phase == TouchPhase.Canceled)
            //                {
            //                    if (Input.touchCount == 1 && mouseOverMapObject != null)
            //                    {
            //                        OnSelectMapObject(mouseOverMapObject, hit.point);
            //                        controlType = ControlType.None;
            //                        return;
            //                    }
            //                }
            //            }

            //#endif

            //            switch (controlType)
            //            {
            //                case ControlType.Move:
            //                    OnInputMove();
            //                    break;
            //                case ControlType.Rotate:
            //                    OnInputRotate();
            //                    break;
            //                case ControlType.Zoom:
            //                    OnInputZoom();
            //                    break;
            //                case ControlType.RotateAndZoom:
            //                    OnInputZoomAndRotate();
            //                    break;
            //            }
        }

//        public void OnInputMove()
//        {
//#if UNITY_STANDALONE_WIN || UNITY_EDITOR
//            if (Input.GetMouseButton(0))
//            {
//                if (touchPos[0] == Input.mousePosition)
//                    return;

//                touchMoveFlag[0] = true;
//                Vector3 delta = Input.mousePosition - touchPos[0];
//                touchPos[0] = Input.mousePosition;

//                float dis;
//                if (viewPlane.Raycast(ray, out dis))
//                {
//                    Vector3 newDragPos = ray.GetPoint(dis);
//                    Vector3 offset = dragePos[0] - newDragPos;
//                    MapRender.Instance.OffsetCamera(offset);
//                }

//            }
//            else if (Input.GetMouseButtonUp(0))
//            {
//                controlType = ControlType.None;
//                if (touchMoveFlag[0])
//                {
//                    touchMoveFlag[0] = false;
//                    return;
//                }

//                if (Physics.Raycast(ray, out hit, 2000, rayCastLayer))
//                {
//                    MapObject mapObjcet = hit.collider.gameObject.GetComponentInParent<MapObject>();
//                    if (mapObjcet != null)
//                    {
//                        OnSelectMapObject(mapObjcet, hit.point);
//                    }
//                    else
//                    {
//                        OnSelectTerrain(hit.point);
//                    }
//                }
//            }
//#else
//            for (int i = 0; i < Input.touchCount; i++)
//            {
//                Touch touch = Input.GetTouch(i);
//                if (touch.phase == TouchPhase.Canceled)
//                {
//                    if (Input.touchCount == 1)
//                    {
//                        touchPos[0] = touch.position;
//                        controlType = ControlType.Move;
//                        break;
//                    }
//                }
//                else if (touch.phase == TouchPhase.Moved)
//                {
//                    if (touchPos[0].Equals(touch.position))
//                        return;

//                    touchMoveFlag[0] = true;
//                    touchPos[0] = touch.position;

//                    float dis;
//                    if (viewPlane.Raycast(ray, out dis))
//                    {
//                        Vector3 newDragPos = ray.GetPoint(dis);
//                        Vector3 offset = dragePos[0] - newDragPos;
//                        MapRender.Instance.OffsetCamera(offset);
//                    }
//                }
//            }
//#endif
//        }
//        public void OnInputRotate()
//        {
//#if UNITY_STANDALONE_WIN || UNITY_EDITOR
//            if (Input.GetMouseButton(1))
//            {
//                if (touchPos[1] == Input.mousePosition)
//                    return;

//                touchMoveFlag[1] = true;
//                Vector3 delta = Input.mousePosition - touchPos[1];
//                touchPos[1] = Input.mousePosition;
//                MapRender.Instance.RotateCamera(delta);
//            }
//            else if (Input.GetMouseButtonUp(1))
//            {
//                controlType = ControlType.None;
//                if (touchMoveFlag[1])
//                {
//                    touchMoveFlag[1] = false;
//                    return;
//                }
//                OnInputCancel();
//            }
//#else




//#endif
//        }
//        public void OnInputZoom()
//        {
//            float offset = Input.GetAxis("Mouse ScrollWheel");
//            MapRender.Instance.ZoomCamera(offset);
//        }


//        public void OnInputZoomAndRotate()
//        {
//#if UNITY_STANDALONE_WIN || UNITY_EDITOR

//#else
//            for (int i = 0; i < Input.touchCount; i++)
//            {
//                Touch touch = Input.GetTouch(i);
//                if (touch.phase == TouchPhase.Canceled)
//                {
//                    controlType = ControlType.None;
//                    return;
//                }
//            }

//            Touch touch1 = Input.GetTouch(0);
//            Touch touch2 = Input.GetTouch(1);

//            Vector2 touch1Dirction = touch1.position - touchPos[0];
//            Vector2 touch2Dirction = touch2.position - touchPos[1];
//            float dotAngle = Vector2.Dot(touch1Dirction, touch2Dirction);
//            if (dotAngle > 0)
//            {
//                // rotate
//                MapRender.Instance.RotateCamera(touch1.deltaPosition);
//            }
//            else
//            {
//                float len = (touch1.position - touch2.position).sqrMagnitude;
//                float srcLen = (touchPos[0] - touchPos[1]).sqrMagnitude;
//                float delta = Mathf.Max(Mathf.Abs(touch2.deltaPosition.x), Mathf.Abs(touch1.deltaPosition.x));
//                if (len < srcLen)
//                    delta = -delta;
//                MapRender.Instance.ZoomCamera(delta / 500f);
//            }
//            touchPos[0] = touch1.position;
//            touchPos[1] = touch2.position;
//#endif
//        }

        public void HandleMobileEvent()
        {

            if (Input.touchCount == 1 && !isRotateMoving)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    dragPosition = touch.position;
                    OnDragWorldBegin();
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    if (dragPosition != Input.mousePosition)
                    {
                        isDragMoving = true;
                        OnDragWorld();
                        dragPosition = Input.mousePosition;
                    }
                }
                else if (touch.phase == TouchPhase.Canceled)
                {
                    if (isDragMoving)
                    {
                        isDragMoving = false;
                        OnDragWorldEnd();
                        return;
                    }
                    OnClickWorld();
                }
            }
            else if (Input.touchCount == 2 && !isDragMoving)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    rotatePosition = Input.mousePosition;
                    OnRotateWorldBegin();
                }
                else if(touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
                {
                    isRotateMoving = true;
                    OnRotateWorld();
                }
                else if (touch1.phase == TouchPhase.Canceled || touch2.phase == TouchPhase.Canceled)
                {
                    if (isRotateMoving)
                    {
                        isRotateMoving = false;
                        OnRotateWorldEnd();
                        return;
                    }
                    OnRClickWorld();
                }
                touchPos[0] = touch1.position;
                touchPos[1] = touch1.position;
            }

            if (Input.GetMouseButton(1) && !isDragMoving)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    rotatePosition = Input.mousePosition;
                    OnRotateWorldBegin();
                }

                else
                {
                    if (rotatePosition != Input.mousePosition)
                    {
                        isRotateMoving = true;
                        OnRotateWorld();
                        rotatePosition = Input.mousePosition;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(1) && !isDragMoving)
            {
                if (isRotateMoving)
                {
                    isRotateMoving = false;
                    OnRotateWorldEnd();
                    return;
                }
                OnRClickWorld();
            }
            else
            {
                Vector2 scrollWheel = Input.mouseScrollDelta;
                if (scrollWheel.y != 0)
                {
                    OnScaleWorld(scrollWheel.y);
                }
            }
        }
    }

}
