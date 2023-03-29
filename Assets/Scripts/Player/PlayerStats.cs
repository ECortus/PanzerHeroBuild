using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

[ExecuteInEditMode]
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; set; }

    [Header("Default stats(on start and handle set-s) ")]
    public float DefaultHP = 100f;
    public float DefaultDamage = 5f;
    public float DefaultTimeReload = 0.5f;
    public int DefaultWhizzbangCount = 3;

    public float MaxHP
    {
        get
        {
            return Modifications.ArmorMod;
        }
    }

    public int MaxWhizzbangCount
    {
        get
        {
            return DefaultWhizzbangCount;
        }
    }

    public float Damage
    {
        get
        {
            return Modifications.DamageMod;
        }
    }

    public float TimeReload
    {
        get
        {
            return Modifications.TimeReloadMod;
        }
    }

    [Space(20)]
    [SerializeField] private UnityEvent DeathEvent;

    private bool _active;
    public bool Active { get { return _active; }}
    public void On()
    {
        _active = true;

        UI.Instance.On();
        ChangePlayType.Instance.Ride();
    }
    public void Off()
    {
        _active = false;

        UI.Instance.Off();
        ChangePlayType.Instance.Disable();
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

    private int _WhizzbangCount;
    public int WhizzbangCount 
    { 
        get { return _WhizzbangCount; }
        set
        {
            _WhizzbangCount = value;
            if(WhizzbangCounter.Instance != null) WhizzbangCounter.Instance.UpdCount();
        }
    }

    void Awake() => Instance = this;

    void Start()
    {
        SetMaxStats();
    }

    public void SetMaxStats()
    {
        HP = MaxHP;
        WhizzbangCount = MaxWhizzbangCount;
    }

    public async void PausePlayerOnTime(int milliseconds)
    {
        Off();
        await UniTask.Delay(milliseconds);
        On();
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
        if(!Active) return;

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
