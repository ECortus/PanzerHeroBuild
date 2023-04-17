using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : DestrictableBuilding
{
    public bool Damaged = false;
    private ActionZone zone;

    private float startCount;

    void Start()
    {
        zone = GetComponentInParent<ActionZone>();
        startCount = destrictableObjects.Count;
    }

    void Update()
    {
        if(destrictableObjects.Count != startCount)
        {
            zone.UpdHouses(this);
            this.enabled = false;
        }
    }
}
