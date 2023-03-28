using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    void Awake() => Instance = this;

    private List<Whizzbang> WhizzbangPool = new List<Whizzbang>();
    private List<ParticleSystem> DestroyEffectPool = new List<ParticleSystem>();

    public GameObject Insert(ObjectType type, GameObject obj, Vector3 pos, Vector3 rot)
    {
        if(type == ObjectType.Whizzbang)
        {
            foreach(Whizzbang wb in WhizzbangPool)
            {
                if(wb == null) continue;

                if(!wb.Active) 
                {
                    wb.Reset(pos, rot);
                    wb.On();
                    return wb.gameObject;
                }
            }

            Whizzbang scr = Instantiate(obj, pos, Quaternion.Euler(rot)).GetComponent<Whizzbang>();
            WhizzbangPool.Add(scr);
            return scr.gameObject;
        }

        if(type == ObjectType.DestroyEffect)
        {
            foreach(ParticleSystem de in DestroyEffectPool)
            {
                if(de == null) continue;

                if(!de.isPlaying) 
                {
                    de.transform.position = pos;
                    de.Play();
                    return de.gameObject;
                }
            }

            ParticleSystem scr = Instantiate(obj, pos, Quaternion.Euler(rot)).GetComponent<ParticleSystem>();
            DestroyEffectPool.Add(scr);
            return scr.gameObject;
        }

        return null;
    }
}

[System.Serializable]
public enum ObjectType
{
    Default, Whizzbang, DestroyEffect
}
