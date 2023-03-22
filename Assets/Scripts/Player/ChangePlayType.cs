using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayType : MonoBehaviour
{
    [SerializeField] private CarController carController;
    [SerializeField] private TankShooting tankShooting;

    public void Ride()
    {
        PlayType.Set(PlayState.Ride);
        carController.enabled = true;
        tankShooting.enabled = false;

        Transform camTarget = GameManager.Instance.rideCamRoot;
        GameManager.Instance.SetFollowTarget(camTarget);
    }

    public void Aim()
    {
        PlayType.Set(PlayState.Aim);
        carController.enabled = false;
        tankShooting.enabled = true;

        Transform camTarget = GameManager.Instance.aimCamRoot;
        GameManager.Instance.SetFollowTarget(camTarget);
    }
}
