using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooting : MonoBehaviour
{
    private float mouseStartX, mouseStartY, diffMouseX, diffMouseY;
    private Vector3 lastCamPos;

    [SerializeField] private float sensivity;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartX = Input.mousePosition.x / Screen.width;
            mouseStartY = Input.mousePosition.y / Screen.height;
            lastCamPos = transform.rotation.eulerAngles;
        }
        if (Input.GetMouseButton(0))
        {
            diffMouseX = (mouseStartX - Input.mousePosition.x / Screen.width)*sensivity;
            diffMouseY = (mouseStartY - Input.mousePosition.y / Screen.height)*sensivity;
            transform.rotation = Quaternion.Euler(lastCamPos.x+diffMouseY,lastCamPos.y-diffMouseX,0);
        }
    }
}
