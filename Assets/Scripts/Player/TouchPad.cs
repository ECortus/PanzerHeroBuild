using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool HaveTouch = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        HaveTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HaveTouch = false;
    }
}
