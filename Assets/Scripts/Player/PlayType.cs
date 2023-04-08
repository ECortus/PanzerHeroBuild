using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayType
{
    private static PlayState State = PlayState.Ride;

    public static PlayState Get() => State;
    public static void Set(PlayState state)
    {
        State = state;

        if(PlayTypeUI.Instance != null) PlayTypeUI.Instance.UpdateButtons();
        TankController.Instance.Touching.StopTouching(true);
    }
}

[System.Serializable]
public enum PlayState
{
    Default, Ride, Aim, Stoped, Disable
}
