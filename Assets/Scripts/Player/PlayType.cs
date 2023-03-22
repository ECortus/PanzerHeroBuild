using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayType
{
    private static PlayState State = PlayState.Default;

    public static PlayState Get() => State;
    public static void Set(PlayState state)
    {
        State = state;
        PlayTypeUI.Instance.UpdateButtons();
    }
}

[System.Serializable]
public enum PlayState
{
    Default, Ride, Aim
}
