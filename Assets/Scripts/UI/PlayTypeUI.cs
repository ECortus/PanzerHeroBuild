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
                aimButt.Open(true);
                break;
            case PlayState.Ride:
                /* rideButt.gameObject.SetActive(false);
                aimButt.gameObject.SetActive(true); */
                rideButt.Close();

                if(Tutorial.Instance != null)
                {
                    if(!Tutorial.Instance.Complete)
                    {
                        if(!Tutorial.Instance.HOLD_isDone) return;
                    }
                }

                aimButt.Open(true);
                break;
            case PlayState.Aim:
                aimButt.Close();

                if(Tutorial.Instance != null)
                {
                    if(!Tutorial.Instance.Complete)
                    {
                        if(!Tutorial.Instance.RIDE_isDone)
                        {
                            rideButt.Close(true);
                            return;
                        }
                    }
                }
            
                if(TankHeadController.Instance.tankShooting.isReloading) rideButt.Close(true);
                else rideButt.Open(true);
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
