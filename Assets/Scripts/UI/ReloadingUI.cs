using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class ReloadingUI : ShowHideUI
{
    [SerializeField] private GameObject whizzbangs;
    [SerializeField] private Slider slider;
    
    public async UniTask StartReloading(int tim)
    {
        /* slider.gameObject.SetActive(true); */
        /* whizzbangs.SetActive(false); */

        WhizzbangUI.Instance.Close();

        StopAllCoroutines();
        StartCoroutine(ShowProcess());

        float wholeTime = tim * PlayerStats.Instance.MaxWhizzbangCount / 1000;

        float time = 0f;
        slider.value = 0f;

        while(true)
        {
            time += Time.deltaTime;
            slider.value = 1f / wholeTime * time;

            if(time >= wholeTime) break;
            await UniTask.Delay(0);
        }

        PlayerStats.Instance.WhizzbangCount = PlayerStats.Instance.MaxWhizzbangCount;

        StopAllCoroutines();
        StartCoroutine(HideProcess());

        WhizzbangUI.Instance.Open();

        /* whizzbangs.SetActive(true); */
        /* slider.gameObject.SetActive(false); */
    }
}
