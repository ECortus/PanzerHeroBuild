using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public static HealthUI Instance { get; set; }

    [SerializeField] private HPBarObject[] bars;
    private float speedLerp = 0.2f;

    private Coroutine coroutine;

    void Awake() => Instance = this;

    private void Start()
    {
        Reset();
        On();
    }

    public void UpdSlid()
    {
        SetBarsHPValue(1f / PlayerStats.Instance.MaxHP * PlayerStats.Instance.HP);
        if(coroutine == null) coroutine = StartCoroutine(Reduce());
        else
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(Reduce());
        }
    }

    void SetBarsHPValue(float value)
    {
        foreach(HPBarObject bar in bars)
        {
            if(bar != null && bar.GameObject.activeInHierarchy)
            {
                bar.HPFill.value = value;
            }
        }
    }

    void SetBarsReduceValue(float value)
    {
        foreach(HPBarObject bar in bars)
        {
            if(bar != null && bar.GameObject.activeInHierarchy)
            {
                bar.ReduceFill.value = value;
            }
        }
    }

    float GetBarsHPValue()
    {
        foreach(HPBarObject bar in bars)
        {
            if(bar != null && bar.GameObject.activeInHierarchy)
            {
                return bar.HPFill.value;
            }
        }

        return 0f;
    }

    float GetBarsReduceValue()
    {
        foreach(HPBarObject bar in bars)
        {
            if(bar != null && bar.GameObject.activeInHierarchy)
            {
                return bar.ReduceFill.value;
            }
        }

        return 0f;
    }

    private IEnumerator Reduce()
    {
        float speed = 0;
        while (GetBarsReduceValue() != GetBarsHPValue())
        {
            speed += Time.deltaTime;
            SetBarsReduceValue(Mathf.Lerp(GetBarsReduceValue(), GetBarsHPValue(), speed * speedLerp));
            if (GetBarsReduceValue() < GetBarsHPValue())
            {
                /* Off(); */
                SetBarsReduceValue(GetBarsHPValue());
                coroutine = null;
                break;
            }
            yield return null;
        }
    }

    public void On()
    {
        foreach(HPBarObject bar in bars)
        {
            if(bar != null && bar.enabled)
            {
                bar.On();
            }
        }
        enabled = true;
    } 

    public void Off()
    {
        foreach(HPBarObject bar in bars)
        {
            if(bar != null && bar.enabled)
            {
                bar.Off();
            }
        }
        enabled = false;
    } 

    public void Reset()
    {
        SetBarsHPValue(1f);
        SetBarsReduceValue(1f);
        HealthUI.Instance.UpdSlid();
    }
}
