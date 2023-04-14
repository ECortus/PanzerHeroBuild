using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandShowHide : ShowHideUI
{
    public void Open()
    {
        if(!isShown)
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

    public void Close()
    {
        if(isShown)
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
