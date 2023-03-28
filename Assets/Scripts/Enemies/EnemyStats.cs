using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStats : MonoBehaviour
{
    [Header("Default stats(on start and handle set-s) ")]
    public float _DefaultHP = 100f;
    public float _DefaultDamage = 5f;

    public float MaxHP
    {
        get
        {
            return _DefaultHP;
        }
    }

    public float Damage
    {
        get
        {
            return _DefaultDamage;
        }
    }

    [SerializeField] private UnityEvent DeathEvent;

    private bool _active;
    public bool Active { get { return _active; }}

    public void On()
    {
        _active = true;
    }
    public void Off()
    {
        _active = false;
    }
    
    private float _HP;
    public float HP 
    { 
        get { return _HP; }
        set
        {
            _HP = value;
        }
    }

    void Start()
    {
        SetMaxStats();
    }

    public void SetMaxStats()
    {
        HP = MaxHP;
    }

    public void Heal(float hl)
    {
        if(hl + HP > MaxHP)
        {
            HP = MaxHP;
        }
        else
        {
            HP += hl;
        }
    }

    public void GetHit(float dmg)
    {
        if(!Active)
        {
            if(dmg >= HP)
            {
                HP = 0;
                Death();
                return;
            }
            
            On();
        }

        if(dmg >= HP)
        {
            HP = 0;
            Death();
        }
        else
        {
            HP -= dmg;
        }
    }

    void Death()
    {
        DeathEvent.Invoke();
    }
}
