using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooting : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject whizzbangPrefab;

    void Update()
    {
        Debug.DrawRay(muzzle.position, muzzle.forward * 100f);
    }

    public void Shoot()
    {
        Vector3 pos = muzzle.position;
        Vector3 rot = muzzle.eulerAngles;
        ObjectPool.Instance.Insert(ObjectType.Whizzbang, whizzbangPrefab, pos, rot);
    }
}
