using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyTankShooting : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject whizzbangPrefab;
    
    private float ReloadTime => 2.3f;

    public void On()
    {
        this.enabled = true;

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
        GameObject go = ObjectPool.Instance.Insert(ObjectType.Whizzbang, whizzbangPrefab, pos, rot);
        go.GetComponent<Whizzbang>().damage = stats.Damage;
    }
}
