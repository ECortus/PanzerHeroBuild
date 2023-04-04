using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayTypeUI : MonoBehaviour
{
    public static PlayTypeUI Instance;

    [SerializeField] private PlayTypeButton rideButt, aimButt;

    void Awake() => Instance = this;

    public void UpdateButtons()
    {
        switch(PlayType.Get())
        {
            case PlayState.Stoped:
                /* rideButt.gameObject.SetActive(false);
                aimButt.gameObject.SetActive(true); */
                rideButt.Close();
                aimButt.Open();
                break;
            case PlayState.Ride:
                /* rideButt.gameObject.SetActive(false);
                aimButt.gameObject.SetActive(true); */
                rideButt.Close();
                aimButt.Open();
                break;
            case PlayState.Aim:
                if(TankHeadController.Instance.Aiming) rideButt.Close();
                else rideButt.Open();

                aimButt.Close();
                break;
            default:
                /* rideButt.gameObject.SetActive(false);
                aimButt.gameObject.SetActive(false); */
                rideButt.Close();
                aimButt.Close();
                break;
        }
    }
}
