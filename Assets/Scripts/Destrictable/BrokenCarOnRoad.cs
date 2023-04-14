using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BrokenCarOnRoad : MonoBehaviour
{
    [SerializeField] private Transform car;
    [SerializeField] private UnityEvent crash;
    [SerializeField] private float bound, duration;

    void OnTriggerEnter(Collider col)
    {
        GameObject go = col.gameObject;
        
        switch(go.tag)
        {
            case "Player":
                Crash();
                break;
            default:
                break;
        }
    }

    void Crash()
    {
        crash.Invoke();
        StartCoroutine(Broke());
    }
    
    public void SpawnEffectOnCenter(GameObject effect)
    {
        if(effect != null) ParticlePool.Instance.Insert(ParticleType.BrokenCarEffect, effect, car.position);
    }

    private float time = 0f;

    IEnumerator Broke()
    {
        Vector3 pos = car.localPosition;
        Vector3 scale = car.localScale;

        float diff = scale.x - bound;

        while(time < duration)
        {
            car.localPosition = pos - new Vector3(0f, 1f, 0f) * pos.y * 0.9f * time / duration;
            car.localScale = scale - new Vector3(0f, 0f, 1f) * diff * time / duration;

            time += Time.deltaTime;

            yield return null;
        }

        yield return null;
    }
}
