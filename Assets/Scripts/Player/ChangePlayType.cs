using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayType : MonoBehaviour
{
    [SerializeField] private TankController carController;
    [SerializeField] private TankHeadController tankShooting;

    public void Stoped()
    {
        PlayType.Set(PlayState.Stoped);
        carController.enabled = false;
        tankShooting.enabled = false;
        
        TouchPad.Instance.Off();
    }

    public void Ride()
    {
        PlayType.Set(PlayState.Ride);
        carController.enabled = true;
        tankShooting.enabled = false;

        Transform camTarget = GameManager.Instance.rideCamRoot;
        GameManager.Instance.SetFollowTarget(camTarget);

        TouchPad.Instance.On();
    }

    public void Aim()
    {
        PlayType.Set(PlayState.Aim);
        carController.enabled = false;
        tankShooting.enabled = true;

        Transform camTarget = GameManager.Instance.prepareToAimCamRoot;
        GameManager.Instance.SetFollowTarget(camTarget);

        TouchPad.Instance.Off();
    }
}
