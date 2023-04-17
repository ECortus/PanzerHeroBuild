using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private EnemyHealthUI healthUI;
    [SerializeField] private ActionZone aggrAll;

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
    [HideInInspector] public bool isDead = false;

    public void On()
    {
        _active = true;
    }
    public void Off()
    {
        _active = false;
    }

    public void OnGameObject()
    {
        gameObject.SetActive(true);
    }
    
    private float _HP;
    public float HP 
    { 
        get { return _HP; }
        set
        {
            _HP = value;
            if(healthUI != null) healthUI.UpdSlid();
        }
    }

    void OnEnable()
    {
        SetMaxStats();
    }

    void Start()
    {
        aggrAll = GetComponentInParent<ActionZone>();
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

                if(aggrAll != null) aggrAll.On();
                return;
            }
            
            On();
            if(aggrAll != null) aggrAll.On();
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
        isDead = true;

        DeathEvent.Invoke();
        if(aggrAll != null) aggrAll.UpdStats(this);
    }
}
