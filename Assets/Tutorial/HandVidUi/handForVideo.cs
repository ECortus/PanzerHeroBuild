using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handForVideo : MonoBehaviour
{
    public float speedup, speeddown;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, speedup);
            Vector2 pos = Input.mousePosition;
            transform.position = pos;
            
        }
        else
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, speeddown);
        }
    }
}
