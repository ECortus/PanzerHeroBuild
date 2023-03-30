using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Instance;
    void Awake() => Instance = this;

    private List<ParticleSystem> WhizzbangEffectPool = new List<ParticleSystem>();
    private List<ParticleSystem> BulletEffectPool = new List<ParticleSystem>();

    public GameObject Insert(ParticleType type, GameObject obj, Vector3 pos, Vector3 rot)
    {
        if(type == ParticleType.WhizzbangEffect)
        {
            foreach(ParticleSystem de in WhizzbangEffectPool)
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
            WhizzbangEffectPool.Add(scr);
            return scr.gameObject;
        }

        if(type == ParticleType.BulletEffect)
        {
            foreach(ParticleSystem de in BulletEffectPool)
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
            BulletEffectPool.Add(scr);
            return scr.gameObject;
        }

        return null;
    }
}

[System.Serializable]
public enum ParticleType
{
    Default, WhizzbangEffect, BulletEffect
}
