using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayType : MonoBehaviour
{
    public static ChangePlayType Instance { get; set; }
    [SerializeField] private TankController tankController;
    [SerializeField] private TankHeadController tankHead;

    [Space]
    [SerializeField] private HPBarObject hpInRide;
    [SerializeField] private HPBarObject hpInAim;

    void Awake() => Instance = this;

    public void Disable()
    {
        PlayType.Set(PlayState.Disable);
        tankController.Off();
        tankHead.Off();

        hpInRide.Off();
        hpInAim.Off();
        HealthUI.Instance.Reset();

        Transform camTarget = GameManager.Instance.rideCamRoot;
        GameManager.Instance.SetFollowTarget(camTarget);
        
        if(TouchPad.Instance != null) TouchPad.Instance.Off();
    }

    public void Stoped()
    {
        PlayType.Set(PlayState.Stoped);
        tankController.Off();
        tankHead.Off();

        hpInRide.Off();
        hpInAim.Off();
        HealthUI.Instance.Reset();

        Transform camTarget = GameManager.Instance.rideCamRoot;
        GameManager.Instance.SetFollowTarget(camTarget);
        
        if(TouchPad.Instance != null) TouchPad.Instance.Off();
    }

    public void Ride()
    {
        PlayType.Set(PlayState.Ride);
        tankController.On();
        tankHead.Off();

        hpInRide.On();
        hpInAim.Off();
        HealthUI.Instance.Reset();

        Transform camTarget = GameManager.Instance.rideCamRoot;
        GameManager.Instance.SetFollowTarget(camTarget);

        if(TouchPad.Instance != null) TouchPad.Instance.On();

        if(Tutorial.Instance != null)
        {
            if(!Tutorial.Instance.Complete)
            {
                if(Tutorial.Instance.AIM_isDone && Tutorial.Instance.RIDE_isDone)
                {
                    Tutorial.Instance.SetState(TutorialState.NONE);
                }
            }
        }
    }

    public void Aim(float aimOnY = 0f)
    {
        PlayType.Set(PlayState.Aim);
        tankController.Off();
        tankHead.On(aimOnY);

        hpInRide.Off();
        hpInAim.On();
        HealthUI.Instance.Reset();

        Transform camTarget = TankHeadController.Instance.prepareToAimRoot;
        GameManager.Instance.SetFollowTarget(camTarget);

        if(TouchPad.Instance != null) TouchPad.Instance.Off();
    }
}
