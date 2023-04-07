using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShotButtonUI : ShowHideUI
{
    public void Reset()
    {
        isShown = true;
        transform.localScale = Vector3.one;
    }

    public void Open()
    {
        StopAllCoroutines();
        StartCoroutine(ShowProcess());
    }

    public void Close()
    {
        /* StopAllCoroutines();
        StartCoroutine(HideProcess()); */
        isShown = false;
        transform.localScale = Vector3.zero;
    }
}
