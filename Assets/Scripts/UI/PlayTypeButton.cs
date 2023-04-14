using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTypeButton : ShowHideUI
{
    public void Open(bool value = false)
    {
        if(value && !isShown)
        {
            StopAllCoroutines();
            StartCoroutine(ShowProcess());
        }
        else
        {
            isShown = true;
            transform.localScale = Vector3.one;
        }
    }

    public void Close(bool value = false)
    {
        if(value && isShown)
        {
            StopAllCoroutines();
            StartCoroutine(HideProcess());
        }
        else
        {
            isShown = false;
            transform.localScale = Vector3.zero;
        }
    }
}
