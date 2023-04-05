using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Instance;
    void Awake() => Instance = this;

    private List<ParticleSystem> WhizzbangEffectPool = new List<ParticleSystem>();
    private List<ParticleSystem> BulletEffectPool = new List<ParticleSystem>();
    private List<ParticleSystem> TankDestroyedEffectPool = new List<ParticleSystem>();
    private List<ParticleSystem> BarrelBoomEffectPool = new List<ParticleSystem>();

    public GameObject Insert(ParticleType type, GameObject obj, Vector3 pos)
    {
        List<ParticleSystem> list = new List<ParticleSystem>();

        switch(type)
        {
            case ParticleType.WhizzbangEffect:
                list = WhizzbangEffectPool;
                break;
            case ParticleType.BulletEffect:
                list = BulletEffectPool;
                break;
            case ParticleType.TankDestroyedEffect:
                list = TankDestroyedEffectPool;
                break;
            case ParticleType.BarrelBoomEffect:
                list = BarrelBoomEffectPool;
                break;
            default:
                return null;
        }

        foreach(ParticleSystem ps in list)
        {
            if(ps == null) continue;

            if(!ps.isPlaying) 
            {
                ps.transform.position = pos;
                ps.Play();
                return ps.gameObject;
            }
        }

        ParticleSystem scr = Instantiate(obj, pos, Quaternion.Euler(Vector3.zero)).GetComponent<ParticleSystem>();
        list.Add(scr);

        switch(type)
        {
            case ParticleType.WhizzbangEffect:
                WhizzbangEffectPool = list;
                break;
            case ParticleType.BulletEffect:
                BulletEffectPool = list;
                break;
            case ParticleType.TankDestroyedEffect:
                TankDestroyedEffectPool = list;
                break;
            case ParticleType.BarrelBoomEffect:
                BarrelBoomEffectPool = list;
                break;
            default:
                return null;
        }

        return scr.gameObject;
    }
}

[System.Serializable]
public enum ParticleType
{
    Default, WhizzbangEffect, BulletEffect, TankDestroyedEffect, BarrelBoomEffect
}
