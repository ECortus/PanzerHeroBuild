using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Tutorial : MonoBehaviour
{
    public static Tutorial Instance { get; set; }

    [SerializeField] private HandShowHide HOLD, CHANGEPLAYTYPE, ROTATE, SHOOT, UPGRADE;
    public bool HOLD_isDone, AIM_isDone, ROTATE_isDone, SHOOT_isDone, RIDE_isDone, UPGRADE_isDone;

    private bool _complete = false;
    public bool Complete
    {
        get
        {
            return _complete;
        }
        set
        {
            _complete = value;
            Condition();
        }
    }

    [Space]
    public TutorialState State = TutorialState.NONE;

    [Space]
    [SerializeField] private EnemyAgrrAll enemies;

    void Awake()
    {
        if(Statistics.LevelIndex >= 1) 
        {
            if(Statistics.ArmorLVL > 0 || Statistics.DamageLVL > 0 || Statistics.TimeReloadLVL > 0)
            {
                gameObject.SetActive(false);
                Instance = null;
                return;
            }

            SetState(TutorialState.UPGRADE, true);
        }
        else
        {
            SetState(TutorialState.NONE);
        }

        Instance = this;
    }

    void Update()
    {
        if(!RIDE_isDone)
        {
            if(HOLD_isDone && ROTATE_isDone && SHOOT_isDone && AIM_isDone)
            {
                if(enemies.Defeated)
                {
                    SetState(TutorialState.RIDE, true);
                }
            }
        }
    }

    public void SetState(TutorialState _state, bool done = false)
    {
        if(_state == TutorialState.NONE) OffAll();

        if(_state == State) return;

        OffAll();
        State = _state;

        switch(State)
        {
            case TutorialState.HOLD:
                if(!HOLD_isDone) HOLD.Open();
                HOLD_isDone = done;
                break;
            case TutorialState.AIM:
                if(!AIM_isDone) CHANGEPLAYTYPE.Open();
                AIM_isDone = done;
                PlayType.Set(PlayState.Ride);
                break;
            case TutorialState.ROTATE:
                if(!ROTATE_isDone) ROTATE.Open();
                ROTATE_isDone = done;
                break;
            case TutorialState.SHOOT:
                if(!SHOOT_isDone) SHOOT.Open();
                SHOOT_isDone = done;
                break;
            case TutorialState.RIDE:
                if(!RIDE_isDone) CHANGEPLAYTYPE.Open();
                RIDE_isDone = done;
                PlayType.Set(PlayState.Aim);
                break;
            case TutorialState.UPGRADE:
                if(!UPGRADE_isDone) UPGRADE.Open();
                UPGRADE_isDone = done;
                break;
            default:
                Debug.Log("looser");
                break;
        }
    }

    void OffAll()
    {
        HOLD.Close();
        CHANGEPLAYTYPE.Close();
        ROTATE.Close();
        SHOOT.Close();
        UPGRADE.Close();
    }

    public async void Condition()
    {
        if(Complete)
        {
            await UniTask.Delay(1500);
            gameObject.SetActive(false);
            Instance = null;
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}

public enum TutorialState
{
    NONE, HOLD, AIM, ROTATE, SHOOT, RIDE, UPGRADE
}

/* if(Tutorial.Instance != null)
            {
                if(!Tutorial.Instance.Complete)
                {
                    Tutorial.Instance.SetState(TutorialState.CHANGEPLAYTYPE);
                }
            } */
