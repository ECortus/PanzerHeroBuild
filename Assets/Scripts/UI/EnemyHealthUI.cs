using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;

    [Space]
    [SerializeField] private Slider HPFill;
    [SerializeField] private Slider ReduceFill;
    private float speedLerp = 0.2f;

    private Coroutine coroutine;

    private void Start()
    {
        Reset();
        On();
    }

    public void UpdSlid()
    {
        HPFill.value = 1f / stats.MaxHP * stats.HP;
        if(coroutine == null) coroutine = StartCoroutine(Reduce());
        else
        {
            StopCoroutine(coroutine);
            coroutine = StartCoroutine(Reduce());
        }
    }

    private IEnumerator Reduce()
    {
        float speed = 0;
        while (ReduceFill.value != HPFill.value)
        {
            speed += Time.deltaTime;
            ReduceFill.value = Mathf.Lerp(ReduceFill.value, HPFill.value, speed * speedLerp);
            if (ReduceFill.value < HPFill.value)
            {
                /* Off(); */
                ReduceFill.value = HPFill.value;
                coroutine = null;
                break;
            }
            yield return null;
        }
    }

    public void On()
    {
        gameObject.SetActive(true);
        enabled = true;
    } 

    public void Off()
    {
        gameObject.SetActive(false);
        enabled = false;
    } 

    public void Reset()
    {
        HPFill.value = 1f;
        ReduceFill.value = 1f;
        HealthUI.Instance.UpdSlid();
    }
}
