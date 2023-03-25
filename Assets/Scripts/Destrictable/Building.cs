using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : DestrictableAction
{
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Whizzbang")
        {
            GetDestroyed(col.transform);
        }
    }
}
