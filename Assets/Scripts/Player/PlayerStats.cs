using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; set; }

    [Header("Default stats(on start and handle set-s) ")]
    public float DefaultHP = 100f;
    public float DefaultDamage = 5f;
    public float DefaultTimeReload = 0.5f;
    public int DefaultWhizzbangCount = 3;

    [Space]
    [SerializeField] private GameObject hitBox;
    [SerializeField] private GameObject unharmedBody;
    [SerializeField] private GameObject harmedBodyPrefab;
    [SerializeField] private Transform harmedParent;

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
            if(HealthUI.Instance != null) HealthUI.Instance.UpdSlid();
        }
    }

    private int _WhizzbangCount;
    public int WhizzbangCount 
    { 
        get { return _WhizzbangCount; }
        set
        {
            _WhizzbangCount = value;
            if(WhizzbangUI.Instance != null) WhizzbangUI.Instance.UpdCount();
        }
    }

    void Awake() => Instance = this;

    void OnEnable()
    {
        SetMaxStats();
    }

    public void SetMaxStats()
    {
        HP = MaxHP;
        if(HealthUI.Instance != null) HealthUI.Instance.Reset();
        
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

            if(PlayType.Get() == PlayState.Ride && Tutorial.Instance != null)
            {
                if(!Tutorial.Instance.Complete)
                {
                    Tutorial.Instance.SetState(TutorialState.AIM, true);
                }
            }
        }
    }

    void Death()
    {
        DeathEvent.Invoke();
    }

    public void ResetBody()
    {
        if(harmed != null)
        {
            harmed.SetActive(false);
            harmed = null;
        }

        unharmedBody.SetActive(true);
        hitBox.SetActive(true);
    }

    GameObject harmed;

    public void InsertDeadBody()
    {
        unharmedBody.SetActive(false);
        hitBox.SetActive(false);

        harmed = Instantiate(harmedBodyPrefab, harmedParent);
    }
}
