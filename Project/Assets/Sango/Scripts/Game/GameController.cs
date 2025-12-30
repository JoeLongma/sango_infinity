using Sango.Game.Player;
using Sango.Game.Render.UI;
using Sango.Render;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sango.Game
{
    public class GameController : Singleton<GameController>
    {
        public bool Enabled { get; set; }
        public bool KeyboardMoveEnabled { get; set; }
        public bool RotateViewEnabled { get; set; }
        public bool DragMoveViewEnabled { get; set; }
        public bool ZoomViewEnabled { get; set; }
        public bool BorderMoveViewEnabled { get; set; }

        enum ControlType : byte
        {
            None = 0,
            Move,
            Rotate,
        }
        ControlType controlType = ControlType.None;

        //public delegate void OnClickCell(Cell cell);
        //public delegate void OnDoubleClickCell(Cell cell);

        public delegate void OnCellOverEnter(Cell cell);
        public delegate void OnCellOverExit(Cell cell);
        public delegate void OnCellOverStay(Cell cell, Vector3 hitPoint, bool isOverUI);

        public delegate void OnCancelHandle();
        public delegate void OnClickHandle(Cell cell);
        //public delegate void OnKeyDown(KeyCode keyCode);

        //public OnClickCell onClickCell;
        //public OnDoubleClickCell onDoubleClickCell;
        public OnCellOverEnter onCellOverEnter;
        public OnCellOverExit onCellOverExit;
        public OnCellOverStay onCellOverStay;
        public OnCancelHandle onCancelHandle;
        public OnClickHandle onClickHandle;
        public OnClickHandle onRClickHandle;

        private Plane viewPlane = new Plane(Vector3.up, Vector3.zero);

        public GameController()
        {
            KeyboardMoveEnabled = true;
            RotateViewEnabled = true;
            DragMoveViewEnabled = true;
            ZoomViewEnabled = true;
            BorderMoveViewEnabled = true;
        }

        public void Reset()
        {
            KeyboardMoveEnabled = true;
            RotateViewEnabled = true;
            DragMoveViewEnabled = true;
            ZoomViewEnabled = true;
            BorderMoveViewEnabled = true;
        }

        public bool IsOverUI()
        {
            return (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())/* || FairyGUI.Stage.isTouchOnUI*/;
        }
        public bool IsOverUI(int fingerId)
        {
            return (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId))/* || FairyGUI.Stage.isTouchOnUI*/;
        }

        public Cell mouseOverCell;
        public Cell touchBeganCell;
        public Vector3 clickPosition;
        private Ray ray;
        private RaycastHit hit;
        private int rayCastLayer = LayerMask.GetMask("Map");

        public Cell CheckMouseIsOnMapCell(Ray ray, out Vector3 hitPoint)
        {
            if (Physics.Raycast(ray, out hit, 2000, rayCastLayer))
            {
                hitPoint = hit.point;
                return Scenario.Cur.Map.GetCell(hitPoint);
            }
            hitPoint = Vector3.zero;
            return null;
        }

        public Cell CheckMouseIsOnMapCell(Vector3 mousePosition, out Vector3 hitPoint)
        {
            ray = Camera.main.ScreenPointToRay(mousePosition);
            return CheckMouseIsOnMapCell(ray, out hitPoint);
        }

        private Vector2[] touchPos = new Vector2[2];
        bool isDragMoving = false;
        bool isRotateMoving = false;
        Vector3 rotatePosition;
        Vector3 dragPosition;

        Vector3 worldPlaneDragPosition;

        public void HandleOverCell()
        {
            if (Scenario.Cur == null) return;

            Cell overCell = CheckMouseIsOnMapCell(Input.mousePosition, out Vector3 hitPoint);
            if (overCell != mouseOverCell)
            {
                if (mouseOverCell != null)
                    onCellOverExit?.Invoke(mouseOverCell);
                mouseOverCell = overCell;
                if (mouseOverCell != null)
                    onCellOverEnter?.Invoke(mouseOverCell);
            }
            else if (overCell == null && overCell == mouseOverCell)
            {
                onCellOverExit?.Invoke(overCell);
            }
            else
            {
                onCellOverStay?.Invoke(overCell, hitPoint, IsOverUI());
            }
        }

        float clickTime = 0.08f;
        float mousePressTime = 0;

        public void HandleWindowsEvent()
        {

            if (Input.GetMouseButton(0) && !isRotateMoving)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    mousePressTime = Time.time;

                    bool isOverUI = IsOverUI();

                    PlayerCommand.Instance.HandleEvent(CommandEventType.ClickDown, mouseOverCell, dragPosition, isOverUI);

                    if (controlType != ControlType.None)
                        return;

                    dragPosition = Input.mousePosition;

                    if (isOverUI)
                    {
                        isDragMoving = true;
                        return;
                    }

                    if (mouseOverCell == null) return;


                    if (!mouseOverCell.IsEmpty())
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

                    float passedTime = Time.time - mousePressTime;

                    if (dragPosition != Input.mousePosition && passedTime > clickTime)
                    {
                        isDragMoving = true;

                        if (DragMoveViewEnabled)
                        {
                            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            float dis;
                            if (viewPlane.Raycast(ray, out dis))
                            {
                                Vector3 newDragPos = ray.GetPoint(dis);
                                Vector3 offset = worldPlaneDragPosition - newDragPos;
                                MapRender.Instance.OffsetCamera(offset);
                            }
                        }
                        dragPosition = Input.mousePosition;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0) && !isRotateMoving)
            {
                //if (controlType != ControlType.Move)
                //    return;
                controlType = ControlType.None;
                bool isOverUI = IsOverUI();

                if (isDragMoving)
                {
                    isDragMoving = false;
                    PlayerCommand.Instance.HandleEvent(CommandEventType.ClickUp, mouseOverCell, dragPosition, isOverUI);
                    return;
                }
                clickPosition = Input.mousePosition;
                OnClickWorld();
                PlayerCommand.Instance.HandleEvent(CommandEventType.ClickUp, mouseOverCell, dragPosition, isOverUI);

            }
            else if (Input.GetMouseButton(1) && !isDragMoving)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    bool isOverUI = IsOverUI();
                    PlayerCommand.Instance.HandleEvent(CommandEventType.RClickDown, mouseOverCell, dragPosition, isOverUI);

                    rotatePosition = Input.mousePosition;
                    if (controlType != ControlType.None)
                        return;

                    if (isOverUI) return;

                    if (mouseOverCell != null && !mouseOverCell.IsEmpty())
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
                        if (RotateViewEnabled)
                        {
                            Vector3 delta = Input.mousePosition - rotatePosition;
                            MapRender.Instance.RotateCamera(delta);
                        }
                        rotatePosition = Input.mousePosition;
                    }
                }
            }
            else if (Input.GetMouseButtonUp(1) && !isDragMoving)
            {
                bool isOverUI = IsOverUI();

                if (controlType != ControlType.Rotate)
                {
                    PlayerCommand.Instance.HandleEvent(CommandEventType.RClickUp, mouseOverCell, dragPosition, isOverUI);
                    return;
                }

                controlType = ControlType.None;

                if (isRotateMoving)
                {
                    isRotateMoving = false;
                    PlayerCommand.Instance.HandleEvent(CommandEventType.RClickUp, mouseOverCell, dragPosition, isOverUI);
                    return;
                }
                clickPosition = Input.mousePosition;
                OnRClickWorld();
                PlayerCommand.Instance.HandleEvent(CommandEventType.RClickUp, mouseOverCell, dragPosition, isOverUI);
            }
            else
            {
                if (ZoomViewEnabled)
                {
                    Vector2 scrollWheel = Input.mouseScrollDelta;
                    if (scrollWheel.y != 0)
                    {
                        MapRender.Instance.ZoomCamera(scrollWheel.y);
                    }
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
                    mousePressTime = Time.time;

                    bool isOverUI = IsOverUI(touch.fingerId);
                    PlayerCommand.Instance.HandleEvent(CommandEventType.ClickDown, mouseOverCell, dragPosition, isOverUI);

                    dragPosition = touch.position;
                    isDragMoving = false;

                    if (isOverUI)
                    {
                        isDragMoving = true;
                        return;
                    }

                    touchBeganCell = CheckMouseIsOnMapCell(touch.position, out Vector3 hitPoint);
                    if (touchBeganCell != null && !touchBeganCell.IsEmpty())
                    {
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

                    float passedTime = Time.time - mousePressTime;
                    if (!dragPosition.Equals(touch.position) && passedTime > clickTime)
                    {
                        isDragMoving = true;

                        if (DragMoveViewEnabled)
                        {
                            ray = Camera.main.ScreenPointToRay(touch.position);
                            float dis;
                            if (viewPlane.Raycast(ray, out dis))
                            {
                                Vector3 newDragPos = ray.GetPoint(dis);
                                Vector3 offset = worldPlaneDragPosition - newDragPos;
                                MapRender.Instance.OffsetCamera(offset);
                            }
                        }
                        dragPosition = touch.position;
                    }
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    //if (controlType != ControlType.Move)
                    //    return;

                    bool isOverUI = IsOverUI(touch.fingerId);

                    controlType = ControlType.None;
                    if (isDragMoving)
                    {
                        isDragMoving = false;
                        PlayerCommand.Instance.HandleEvent(CommandEventType.ClickUp, mouseOverCell, dragPosition, isOverUI);
                        return;
                    }

                    touchBeganCell = CheckMouseIsOnMapCell(touch.position, out Vector3 hitPoint);
                    if (touchBeganCell != mouseOverCell)
                    {
                        if (mouseOverCell != null)
                            onCellOverExit?.Invoke(mouseOverCell);
                        mouseOverCell = touchBeganCell;
                        if (mouseOverCell != null)
                            onCellOverEnter?.Invoke(mouseOverCell);
                    }
                    else if (touchBeganCell != null && touchBeganCell == mouseOverCell)
                    {
                        onCellOverExit?.Invoke(touchBeganCell);
                    }
                    clickPosition = touch.position;
                    OnClickWorld();
                    PlayerCommand.Instance.HandleEvent(CommandEventType.ClickUp, mouseOverCell, dragPosition, isOverUI);

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

                    // 移动端不提供右键
                    //OnRClickWorld();
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
                        if (RotateViewEnabled)
                            MapRender.Instance.RotateCamera(touch1.deltaPosition);
                    }
                    else
                    {
                        float len = (touch1.position - touch2.position).sqrMagnitude;
                        float srcLen = (touchPos[0] - touchPos[1]).sqrMagnitude;
                        float delta = Mathf.Max(Mathf.Abs(touch2.deltaPosition.x), Mathf.Abs(touch1.deltaPosition.x));
                        if (len < srcLen)
                            delta = -delta;
                        if (ZoomViewEnabled)
                            MapRender.Instance.ZoomCamera(delta / 100f);
                    }
                    touchPos[0] = touch1.position;
                    touchPos[1] = touch2.position;
                }
            }
        }


        bool[] keyFlags = new bool[4];
        bool hasKey = false;
        private bool MoveCameraKeyBoard()
        {
            if (!KeyboardMoveEnabled) return true;

            if (hasKey)
            {
                Array.Clear(keyFlags, 0, 4);
                hasKey = false;
            }

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

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                OnCancel();
            }

            if (hasKey)
            {
                MapRender.Instance.MoveCameraKeyBoard(keyFlags);
                if (controlType == ControlType.Move)
                    controlType = ControlType.None;
            }

            return hasKey;
        }

        float boderWidth = 10;
        public void HandleBorderMoveCamera()
        {
#if UNITY_EDITOR
            return;
#endif

            if (!BorderMoveViewEnabled) return;

            int sWidth = UnityEngine.Screen.width;
            int sHeight = UnityEngine.Screen.height;

            if (hasKey)
            {
                Array.Clear(keyFlags, 0, 4);
                hasKey = false;
            }

            // 左下0,0 右上max max
            // 上 height > 
            if (Input.mousePosition.y > sHeight - boderWidth)
            {
                // 上
                keyFlags[2] = true;
                hasKey = true;
            }
            else if (Input.mousePosition.y < boderWidth)
            {
                // 下
                keyFlags[3] = true;
                hasKey = true;
            }

            if (Input.mousePosition.x > sWidth - boderWidth)
            {
                // 右
                keyFlags[1] = true;
                hasKey = true;
            }
            else if (Input.mousePosition.x < boderWidth)
            {
                // 左
                keyFlags[0] = true;
                hasKey = true;
            }

            if (hasKey)
            {
                MapRender.Instance.MoveCameraKeyBoard(keyFlags);
                if (controlType == ControlType.Move)
                    controlType = ControlType.None;
            }

        }

        public void Update()
        {
            PlayerCommand.Instance.Update();

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            HandleOverCell();
#endif
            if (!Enabled) return;

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            HandleWindowsEvent();
            MoveCameraKeyBoard();
            HandleBorderMoveCamera();
#else
            HandleMobileEvent();
#endif
        }
        public void OnClickWorld()
        {
            if (onClickHandle != null && mouseOverCell != null)
            {
                onClickHandle.Invoke(mouseOverCell);
                return;
            }
            if (mouseOverCell != null)
                PlayerCommand.Instance.HandleEvent(CommandEventType.Click, mouseOverCell, clickPosition, false);
        }

        public void OnRClickWorld()
        {
            if (onRClickHandle != null && mouseOverCell != null)
            {
                onRClickHandle.Invoke(mouseOverCell);
                return;
            }

            if (mouseOverCell != null)
                PlayerCommand.Instance.HandleEvent(CommandEventType.RClick, mouseOverCell, clickPosition, false);
        }

        public void OnCancel()
        {
            if (onCancelHandle != null)
            {
                onCancelHandle.Invoke();
                return;
            }

            PlayerCommand.Instance.HandleEvent(CommandEventType.Cancel, null, clickPosition, false);
            //Sango.Game.Render.UI.ContextMenu.Close();
        }


    }

}
