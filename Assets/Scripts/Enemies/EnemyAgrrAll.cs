using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgrrAll : MonoBehaviour
{
    [SerializeField] private GameObject eventPoint;
    [SerializeField] private List<EnemyStats> stats;

    public void On()
    {
        foreach(EnemyStats stat in stats)
        {
            stat.On();
        }
    }

    public void Upd(EnemyStats stat)
    { 
        if(eventPoint == null) return;

        stats.Remove(stat);

        if(stats.Count == 0) eventPoint.SetActive(false);
    }
}
