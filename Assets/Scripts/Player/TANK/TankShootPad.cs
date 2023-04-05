using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

public class TankShootPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static TankShootPad Instance { get; set; }

    public bool HaveTouch = false;
    [SerializeField] private TankShotButtonUI button;

    public async void On()
    {
        button.Open();
        await UniTask.Delay(200);

        this.enabled = true;
    }
    public void Off()
    {
        button.Close();
        this.enabled = false;
    }

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
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HaveTouch = false;
    }

    public bool IsPointerOverUIObject() 
    {
        if(!this.enabled) return false;

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        
        return results.Count > 0;
    }
}
