using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static TouchPad Instance { get; set; }

    [SerializeField] private TankTouching tankTouching;

    public bool HaveTouch = false;

    public void On() => gameObject.SetActive(true);
    public void Off() => gameObject.SetActive(false);

    void Awake() => Instance = this;

    void OnEnable()
    {
        HaveTouch = false;
    }

    void OnDisable()
    {
        HaveTouch = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HaveTouch = true;
        if(tankTouching != null)  tankTouching.Acceleration();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HaveTouch = false;
        if(tankTouching != null)  tankTouching.Steering();
    }

    public bool IsPointerOverUIObject() 
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
