using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ZatupMoment : MonoBehaviour
{
    [SerializeField] private float limit = 10f;
    float timer = 0f;

    public static bool DelayOn = false;

    bool anyInput
    {
        get
        {
            return Input.GetMouseButton(0);
        }
    }

    void Update()
    {
        if(DelayOn && PlayType.Get() != PlayState.Ride)
        {
            DelayOn = false;
            Tutorial.Instance.SetState(TutorialState.NONE);
        }

        if(!Tutorial.Instance.Complete || !GameManager.Instance.GetActive() || DelayOn) return;

        if(!anyInput)
        {
            timer += Time.deltaTime;

            if(timer >= limit && Tutorial.Instance.State == TutorialState.NONE)
            {
                OnTip();
                timer = -99999f;
            }
        }
        else
        {
            if(timer < 0f)
            {
                Tutorial.Instance.SetState(TutorialState.NONE);
            }

            timer = 0f;
        }
    }

    void OnTip()
    {
        if(PlayType.Get() == PlayState.Aim)
        {
            SetShootTip();
        }
        else if(PlayType.Get() == PlayState.Ride)
        {
            SetRideTip();
        }
    }

    void SetRideTip()
    {
        if(!Tutorial.Instance.Complete || !GameManager.Instance.GetActive()) return;

        Tutorial.Instance.SetState(TutorialState.HOLD);
    }

    void SetShootTip()
    {
        if(!Tutorial.Instance.Complete || !GameManager.Instance.GetActive()) return;

        Tutorial.Instance.SHOOT_isDone = false;
        Tutorial.Instance.SetState(TutorialState.ROTATE);
    }

    void SetAimTip()
    {
        if(!Tutorial.Instance.Complete || !GameManager.Instance.GetActive()) return;

        /* Tutorial.Instance.AIM_isDone = false; */
        Tutorial.Instance.SetState(TutorialState.AIM, true);
    }

    public async void SetAimTipWithDelay(int delay)
    {
        if(!Tutorial.Instance.Complete || !GameManager.Instance.GetActive()) return;

        DelayOn = true;

        SetAimTip();
        await UniTask.Delay(delay);

        if(!DelayOn) return;

        DelayOn = false;
        Tutorial.Instance.SetState(TutorialState.NONE);
    }
}
