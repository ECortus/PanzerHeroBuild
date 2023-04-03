using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitShooting : MonoBehaviour
{
    [SerializeField] private EnemyUnit unit;
    [SerializeField] private EnemyStats stats;
    [SerializeField] private Transform pivot, muzzle;
    [SerializeField] private GameObject bulletPrefab;
    
    private float ReloadTime => 0.3f;

    public void On()
    {
        this.enabled = true;
        pivot.transform.up = unit.direction;

        if(coroutine == null) coroutine = StartCoroutine(Shooting());
    }
    public void Off()
    {
        this.enabled = false;

        if(coroutine != null) StopCoroutine(coroutine);
        coroutine = null;
    }

    Coroutine coroutine;

    IEnumerator Shooting()
    {
        WaitForSeconds wait = new WaitForSeconds(ReloadTime);
        yield return null;
        
        while(true)
        {
            yield return wait;
            Shoot();
        }
    }

    public void Shoot()
    {
        Vector3 pos = muzzle.position;
        Vector3 rot = muzzle.eulerAngles;
        GameObject go = ObjectPool.Instance.Insert(ObjectType.Bullet, bulletPrefab, pos, rot);
        go.GetComponent<Bullet>().damage = stats.Damage;
    }
}
