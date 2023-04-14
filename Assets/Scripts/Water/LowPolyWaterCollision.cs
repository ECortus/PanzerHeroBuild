using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LowPolyWaterSpace;

public class LowPolyWaterCollision : MonoBehaviour
{
    private bool Someone = false;
    [SerializeField] private WaterNoise water;
    [SerializeField] private float waveLenght = 3f;

    void OnTriggerEnter(Collider col)
    {
        GameObject go = col.gameObject;
        
        switch(go.tag)
        {
            case "Player":
                Someone = true;
                /* water.length = waveLenght; */
                break;
            default:
                break;
        }
    }

    void OnTriggerStay(Collider col)
    {
        GameObject go = col.gameObject;
        
        switch(go.tag)
        {
            case "Player":
                water.WaveOrigin.position = col.transform.position;
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if(!Someone)
        {
            water.WaveOrigin.localPosition = Vector3.Lerp(water.WaveOrigin.localPosition, Vector3.zero, 2f * Time.deltaTime);
            water.length = Mathf.Lerp(water.length, 0f, Time.deltaTime);
        }
        else
        {
            water.length = Mathf.Lerp(water.length, waveLenght, Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider col)
    {
        GameObject go = col.gameObject;
        
        switch(go.tag)
        {
            case "Player":
                Someone = false;
                break;
            default:
                break;
        }
    }
}
