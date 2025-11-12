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
            Move,
            Rotate,
        }
        ControlType controlType = ControlType.None;

        //public delegate void OnClickCell(Cell cell);
        //public delegate void OnDoubleClickCell(Cell cell);

        //public delegate void OnMouseOverEnter(Cell cell);
        //public delegate void OnMouseOverExit(Cell cell);

        //public delegate void OnCancel();
        //public delegate void OnKeyDown(KeyCode keyCode);

        //public OnClickCell onClickCell;
        //public OnDoubleClickCell onDoubleClickCell;
        //public OnCancel onCancel;
        //public OnKeyDown onKeyDown;

        private Plane viewPlane = new Plane(Vector3.up, Vector3.zero);

        public GameController()
        {
            GameEvent.OnContextMenuShow += OnContextMenuShow;
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

        public MapObject mouseOverMapObject;
        public Cell mouseOverCell;
        private Ray ray;
        private RaycastHit hit;
        private int rayCastLayer = LayerMask.GetMask("Map", "Building", "Troops");

        public MapObject CheckMouseIsOnMapObject(Vector3 mousePosition, out Vector3 hitPiont)
        {
            ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out hit, 2000, rayCastLayer))
            {
                hitPiont = hit.point;
                MapObject mapObjcet = hit.collider.gameObject.GetComponentInParent<MapObject>();
                if (mapObjcet != null)
                {
                    return mapObjcet;
                }
            }
            hitPiont = Vector3.zero;
            return null;
        }

        private Vector2[] touchPos = new Vector2[2];
        bool isDragMoving = false;
        bool isRotateMoving = false;
        Vector3 rotatePosition;
        Vector3 dragPosition;

        Vector3 worldPlaneDragPosition;

        public void HandleWindowsEvent()
        {
            MapObject mapObject = CheckMouseIsOnMapObject(Input.mousePosition, out Vector3 hitPoint);
            Cell overCell;
            if (mapObject != null)
                overCell = Scenario.Cur.Map.GetCell(mapObject.position);
            else
                overCell = Scenario.Cur.Map.GetCell(hitPoint);

            if (overCell != mouseOverCell)
            {
                if (mouseOverCell != null)
                    OnCellOverExit(mouseOverCell);
                mouseOverCell = overCell;
                if (mouseOverCell != null)
                    OnCellOverExit(mouseOverCell);
            }
            else if (overCell != null && overCell == mouseOverCell)
            {
                OnCellOverExit(overCell);
            }

            if (mapObject != mouseOverMapObject)
            {
                if (mouseOverMapObject != null)
                    OnMapObjectOverExit(mouseOverMapObject);
                mouseOverMapObject = mapObject;
                if (mouseOverMapObject != null)
                    OnMapObjectOverEnter(mouseOverMapObject);
            }
            else if (mapObject != null && mapObject == mouseOverMapObject)
            {
                OnMapObjectOverStay(mapObject);
            }

            if (Input.GetMouseButton(0) && !isRotateMoving)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (controlType != ControlType.None)
                        return;

                    dragPosition = Input.mousePosition;

                    if (IsOverUI()) return;

                    if (mouseOverMapObject != null)
                        return;

                    //downSelectMapObject = CheckMouseIsOnMapObject(Input.mousePosition, out Vector3 hitPiont);
                    //if (downSelectMapObject != null)
                    //    return;

                    controlType = ControlType.Move;
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    float dis;
                    if (viewPlane.Raycast(ray, out dis))
                        worldPlaneDragPosition = ray.GetPoint(dis);
                }
                else
                {
                    if (controlType != ControlType.Move)
                        return;

                    if (dragPosition != Input.mousePosition)
                    {
                        isDragMoving = true;
                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        float dis;
                        if (viewPlane.Raycast(ray, out dis))
                        {
                            Vector3 newDragPos = ray.GetPoint(dis);
                            Vector3 offset = worldPlaneDragPosition - newDragPos;
                            MapRender.Instance.OffsetCamera(offset);
                        }
                        dragPosition = Input.mousePosition;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0) && !isRotateMoving)
            {
                if (controlType != ControlType.Move)
                    return;
                controlType = ControlType.None;
                if (isDragMoving)
                {
                    isDragMoving = false;
                    return;
                }
                OnClickWorld();
            }
            else if (Input.GetMouseButton(1) && !isDragMoving)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    rotatePosition = Input.mousePosition;
                    if (controlType != ControlType.None)
                        return;

                    if (IsOverUI()) return;

                    if (mouseOverMapObject != null)
                        return;

                    //downSelectMapObject = CheckMouseIsOnMapObject(Input.mousePosition, out Vector3 hitPiont);
                    //if (downSelectMapObject != null)
                    //    return;

                    controlType = ControlType.Rotate;
                }
                else
                {
                    if (controlType != ControlType.Rotate)
                        return;

                    if (rotatePosition != Input.mousePosition)
                    {
                        isRotateMoving = true;
                        Vector3 delta = Input.mousePosition - rotatePosition;
                        MapRender.Instance.RotateCamera(delta);
                        rotatePosition = Input.mousePosition;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(1) && !isDragMoving)
            {
                if (controlType != ControlType.Rotate)
                    return;

                controlType = ControlType.None;

                if (isRotateMoving)
                {
                    isRotateMoving = false;
                    return;
                }

                OnRClickWorld();
            }
            else
            {
                Vector2 scrollWheel = Input.mouseScrollDelta;
                if (scrollWheel.y != 0)
                {
                    MapRender.Instance.ZoomCamera(scrollWheel.y);
                }
            }
        }
        public void HandleMobileEvent()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    dragPosition = touch.position;
                    isDragMoving = false;

                    if (IsOverUI(touch.fingerId)) return;

                    MapObject mapObject = CheckMouseIsOnMapObject(touch.position, out Vector3 hitPoint);
                    Cell overCell;
                    if (mapObject != null)
                        overCell = Scenario.Cur.Map.GetCell(mapObject.position);
                    else
                        overCell = Scenario.Cur.Map.GetCell(hitPoint);

                    if (overCell != mouseOverCell)
                    {
                        if (mouseOverCell != null)
                            OnCellOverExit(mouseOverCell);
                        mouseOverCell = overCell;
                        if (mouseOverCell != null)
                            OnCellOverExit(mouseOverCell);
                    }
                    else if (overCell != null && overCell == mouseOverCell)
                    {
                        OnCellOverExit(overCell);
                    }

                    if (mapObject != null)
                    {
                        if (mapObject != mouseOverMapObject)
                        {
                            if (mouseOverMapObject != null)
                                OnMapObjectOverExit(mouseOverMapObject);
                            mouseOverMapObject = mapObject;
                            if (mouseOverMapObject != null)
                                OnMapObjectOverEnter(mouseOverMapObject);
                        }
                        return;
                    }

                    controlType = ControlType.Move;
                    ray = Camera.main.ScreenPointToRay(touch.position);
                    float dis;
                    if (viewPlane.Raycast(ray, out dis))
                        worldPlaneDragPosition = ray.GetPoint(dis);
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    if (controlType != ControlType.Move)
                        return;

                    if (!dragPosition.Equals(touch.position))
                    {
                        isDragMoving = true;
                        ray = Camera.main.ScreenPointToRay(touch.position);
                        float dis;
                        if (viewPlane.Raycast(ray, out dis))
                        {
                            Vector3 newDragPos = ray.GetPoint(dis);
                            Vector3 offset = worldPlaneDragPosition - newDragPos;
                            MapRender.Instance.OffsetCamera(offset);
                        }
                        dragPosition = touch.position;
                    }
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (controlType != ControlType.Move)
                        return;

                    controlType = ControlType.None;
                    if (isDragMoving)
                    {
                        isDragMoving = false;
                        return;
                    }



                    OnClickWorld();
                }
            }
            else if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    // 两只手指操作可以无视界面旋转和缩放相机
                    //if ((touch1.phase == TouchPhase.Began && IsOverUI(touch1.fingerId)) || (touch2.phase == TouchPhase.Began && IsOverUI(touch2.fingerId)))
                    //{
                    //    controlType = ControlType.None;
                    //    return;
                    //}
                    touchPos[0] = touch1.position;
                    touchPos[1] = touch2.position;
                    controlType = ControlType.Rotate;
                }
                // 需要先检测,不然会由于有一个是Move而导致检测不到End
                else if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled || touch2.phase == TouchPhase.Canceled)
                {
                    // 返回单手指移动
                    controlType = ControlType.Move;

                    // 不再触发Click
                    isDragMoving = true;

                    Vector3 touchPosition;
                    if (touch1.phase != TouchPhase.Ended && touch1.phase != TouchPhase.Canceled)
                    {
                        touchPosition = touch1.position;
                    }
                    else
                    {
                        touchPosition = touch2.position;
                    }
                    ray = Camera.main.ScreenPointToRay(touchPosition);
                    float dis;
                    if (viewPlane.Raycast(ray, out dis))
                        worldPlaneDragPosition = ray.GetPoint(dis);

                    if (isRotateMoving)
                    {
                        isRotateMoving = false;
                        return;
                    }

                    OnRClickWorld();
                }
                else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    //if (controlType != ControlType.Rotate)
                    //    return;

                    isRotateMoving = true;
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
                        MapRender.Instance.ZoomCamera(delta / 100f);
                    }
                    touchPos[0] = touch1.position;
                    touchPos[1] = touch2.position;
                }
            }
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
        }
        public void OnClickWorld()
        {
            //Sango.Log.Error("OnClickWorld");

        }

        public void OnRClickWorld()
        {
            //Sango.Log.Error("OnRClickWorld");
        }

        public void OnMapObjectOverEnter(MapObject mapObject)
        {

        }
        public void OnMapObjectOverExit(MapObject mapObject)
        {

        }

        public void OnMapObjectOverStay(MapObject mapObject)
        {

        }
        public void OnCellOverEnter(Cell cell)
        {

        }
        public void OnCellOverExit(Cell cell)
        {

        }

        public void OnCellOverStay(Cell cell)
        {

        }

        public void OnCancel()
        {
            //Sango.Game.Render.UI.ContextMenu.Close();
        }
    }

}
