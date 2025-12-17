using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventPass : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public bool nextOnly = true; 

    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> func) where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        GameObject currObj = gameObject;
        int currIndex = -1;
        for(int i = 0; i < results.Count; i++)
        {
            if (currIndex >= 0)
            {
                ExecuteEvents.Execute(results[i].gameObject, data, func);
                if (nextOnly)
                    break;
            }
            if (currObj == results[i].gameObject)
                currIndex = i;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerClickHandler);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerDownHandler);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerUpHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.dragHandler);
    }
}
