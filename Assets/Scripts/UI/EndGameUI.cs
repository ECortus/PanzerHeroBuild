using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameUI : ShowHideUI
{
    [SerializeField] private TextMeshProUGUI moneyText;

    public void Open()
    {
        /* gameObject.SetActive(true); */

        StopAllCoroutines();
        StartCoroutine(ShowProcess());

        moneyText.text = $"+{Statistics.Money - LevelManager.Instance.ActualLevel.moneyOnStart}";
    }

    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(HideProcess());

        /* gameObject.SetActive(false); */
    }
}
