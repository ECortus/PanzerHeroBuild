using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZatupMoment : MonoBehaviour
{
    [SerializeField] private float limit = 10f;
    float timer = 0f;

    bool anyInput
    {
        get
        {
            return Input.GetMouseButton(0);
        }
    }

    void Update()
    {
        if(!Tutorial.Instance.Complete && !GameManager.Instance.GetActive()) return;

        if(!anyInput)
        {
            timer += Time.deltaTime;

            if(timer >= limit)
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
        Tutorial.Instance.SetState(TutorialState.HOLD);
    }

    void SetShootTip()
    {
        Tutorial.Instance.SHOOT_isDone = false;
        Tutorial.Instance.SetState(TutorialState.ROTATE);
    }
}
