using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAgrrAll : MonoBehaviour
{
    public bool Defeated => stats.Count == 0;
    [SerializeField] private GameObject eventPoint;
    [SerializeField] private List<EnemyStats> stats = new List<EnemyStats>();

    void Start()
    {
        stats.Clear();
        stats = transform.GetComponentsInChildren<EnemyStats>().ToList();
    }

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
