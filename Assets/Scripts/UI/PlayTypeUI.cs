using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTypeUI : MonoBehaviour
{
    public static PlayTypeUI Instance;

    [SerializeField] private GameObject rideButt, aimButt;

    void Awake() => Instance = this;

    public void UpdateButtons()
    {
        switch(PlayType.Get())
        {
            case PlayState.Stoped:
                rideButt.SetActive(false);
                aimButt.SetActive(true);
                break;
            case PlayState.Ride:
                rideButt.SetActive(false);
                aimButt.SetActive(true);
                break;
            case PlayState.Aim:
                if(TankHeadController.Instance.Aiming) rideButt.SetActive(false);
                else rideButt.SetActive(true);

                aimButt.SetActive(false);
                break;
            default:
                rideButt.SetActive(false);
                aimButt.SetActive(false);
                break;
        }
    }
}
