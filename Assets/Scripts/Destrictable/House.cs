using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : DestrictableBuilding
{
    public bool Damaged = false;
    [HideInInspector] public ActionZone zone;

    private float startCount;

    void Start()
    {
        RefreshComponentsToParts();

        /* zone = GetComponentInParent<ActionZone>(); */
        startCount = destrictableObjects.Count;
    }

    void Update()
    {
        if(destrictableObjects.Count != startCount)
        {
            if(zone != null) zone.UpdHouses(this);
            this.enabled = false;
        }
    }
}
