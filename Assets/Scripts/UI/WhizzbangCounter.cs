using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WhizzbangCounter : MonoBehaviour
{
    public static WhizzbangCounter Instance { get; set; }

    private int previousCount = 0;
    private int count => PlayerStats.Instance.WhizzbangCount;

    [SerializeField] private Transform grid;

    void Awake() => Instance = this;

    void OnEnable()
    {
        UpdCount();
    }

    public void UpdCount()
    {
        int diff = count - previousCount;
        int iters = Mathf.Abs(diff);

        if(iters == 0) return;

        bool status = false;

        if(diff > 0)
        {  
            status = true;
        }
        else if (diff < 0)
        {
            status = false;
        }

        GameObject go;
        foreach(Transform child in grid)
        {
            go = child.gameObject;
            if(go.activeSelf == !status)
            {
                go.SetActive(status);
                iters--;

                if(iters == 0) break;
            }
        }

        previousCount = count;
    }
}
