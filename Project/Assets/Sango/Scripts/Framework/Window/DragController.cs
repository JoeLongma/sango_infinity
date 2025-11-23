using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform tran;
    private RectTransform parentTran;
    private Vector2 pointerOffset;

    private void Awake()
    {
        tran = GetComponent<RectTransform>();
        parentTran = tran.parent.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTran, eventData.position, Sango.Game.Game.Instance.UICamera, out Vector2 localPoint);

        // 计算触摸点与拖动对象的偏移量
        pointerOffset = localPoint - (Vector2)tran.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentTran, eventData.position, Sango.Game.Game.Instance.UICamera, out Vector2 localPoint);

        // 更新拖动对象的位置
        tran.anchoredPosition = localPoint - pointerOffset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 重置偏移量
        pointerOffset = Vector2.zero;
    }
}

