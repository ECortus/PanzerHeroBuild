using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTypeButton : ShowHideUI
{
    public void Open()
    {
        /* if(gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(ShowProcess());
        }
        else
        { */
            isShown = true;
            transform.localScale = Vector3.one;
        /* } */
    }

    public void Close()
    {
        /* if(gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(HideProcess());
        }
        else
        { */
            isShown = false;
            transform.localScale = Vector3.zero;
        /* } */
    }
}
