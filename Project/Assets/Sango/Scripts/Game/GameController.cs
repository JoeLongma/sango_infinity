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

        public delegate void OnCancelHandle();
        public delegate void OnClickHandle(Cell cell);
        //public delegate void OnKeyDown(KeyCode keyCode);

        //public OnClickCell onClickCell;
        //public OnDoubleClickCell onDoubleClickCell;
        public OnCellOverEnter onCellOverEnter;
        public OnCellOverExit onCellOverExit;
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
            Cell overCell = CheckMouseIsOnMapCell(Input.mousePosition, out Vector3 hitPoint);
            if (overCell != mouseOverCell)
            {
                if (mouseOverCell != null)
                    onCellOverExit?.Invoke(mouseOverCell);
                mouseOverCell = overCell;
                if (mouseOverCell != null)
                    onCellOverEnter?.Invoke(mouseOverCell);
            }
            else if (overCell != null && overCell == mouseOverCell)
            {
                onCellOverExit?.Invoke(overCell);
            }
        }

        public void HandleWindowsEvent()
        {

            if (Input.GetMouseButton(0) && !isRotateMoving)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (controlType != ControlType.None)
                        return;

                    dragPosition = Input.mousePosition;

                    if (IsOverUI()) return;

                    if (mouseOverCell != null && !mouseOverCell.IsEmpty())
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
                if (isDragMoving)
                {
                    isDragMoving = false;
                    return;
                }
                clickPosition = Input.mousePosition;
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

                    if (!dragPosition.Equals(touch.position))
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
                    if (controlType != ControlType.Move)
                        return;

                    controlType = ControlType.None;
                    if (isDragMoving)
                    {
                        isDragMoving = false;
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
        private bool MoveCameraKeyBoard()
        {
            if (!KeyboardMoveEnabled) return true;

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

        public void HandleWindowsCommandEvent()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsOverUI()) return;
                if (mouseOverCell == null)
                    return;

                clickPosition = Input.mousePosition;
                PlayerCommand.Instance.HandleEvent(CommandEventType.ClickDown, mouseOverCell, clickPosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (IsOverUI()) return;
                if (mouseOverCell == null)
                    return;

                clickPosition = Input.mousePosition;
                PlayerCommand.Instance.HandleEvent(CommandEventType.ClickUp, mouseOverCell, clickPosition);
                PlayerCommand.Instance.HandleEvent(CommandEventType.Click, mouseOverCell, clickPosition);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                clickPosition = Input.mousePosition;
                PlayerCommand.Instance.HandleEvent(CommandEventType.Cancel, mouseOverCell, clickPosition);
            }
        }

        public void HandleMobileCommandEvent()
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    clickPosition = touch.position;
                    PlayerCommand.Instance.HandleEvent(CommandEventType.ClickDown, mouseOverCell, clickPosition);
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    clickPosition = touch.position;
                    PlayerCommand.Instance.HandleEvent(CommandEventType.ClickUp, mouseOverCell, clickPosition);
                    PlayerCommand.Instance.HandleEvent(CommandEventType.Click, mouseOverCell, clickPosition);
                }
            }
            else if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);
                if (touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled)
                {
                    clickPosition = touch1.position;
                    PlayerCommand.Instance.HandleEvent(CommandEventType.Cancel, mouseOverCell, clickPosition);
                }
                else if (touch2.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Canceled)
                {
                    clickPosition = touch2.position;
                    PlayerCommand.Instance.HandleEvent(CommandEventType.Cancel, mouseOverCell, clickPosition);
                }
            }
        }

        public void Update()
        {
            PlayerCommand.Instance.Update();

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            HandleOverCell();
#endif
            if (!Enabled)
            {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
                HandleWindowsCommandEvent();
#else
                HandleMobileCommandEvent();
#endif
            }

            if (Enabled)
            {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
                HandleWindowsEvent();
                MoveCameraKeyBoard();
#else
                HandleMobileEvent();
#endif
            }
        }
        public void OnClickWorld()
        {
            if (onClickHandle != null && mouseOverCell != null)
            {
                onClickHandle.Invoke(mouseOverCell);
                return;
            }
            if (mouseOverCell != null)
                PlayerCommand.Instance.HandleEvent(CommandEventType.Click, mouseOverCell, clickPosition);
        }

        public void OnRClickWorld()
        {
            if (onRClickHandle != null && mouseOverCell != null)
            {
                onRClickHandle.Invoke(mouseOverCell);
                return;
            }

            if (mouseOverCell != null)
                PlayerCommand.Instance.HandleEvent(CommandEventType.RClick, mouseOverCell, clickPosition);
        }

        public void OnCancel()
        {
            if (onCancelHandle != null)
            {
                onCancelHandle.Invoke();
                return;
            }

            PlayerCommand.Instance.HandleEvent(CommandEventType.Cancel, null, clickPosition);
            //Sango.Game.Render.UI.ContextMenu.Close();
        }


    }

}
