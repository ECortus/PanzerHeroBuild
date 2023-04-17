using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActionZone : MonoBehaviour
{
    public bool Defeated => stats.Count == 0;
    [SerializeField] private GameObject eventPoint;
    [SerializeField] private List<EnemyStats> stats = new List<EnemyStats>();

    [Space]
    [SerializeField] private List<House> houses = new List<House>();
    [SerializeField] private int reward = 100;

    void Start()
    {
        stats.Clear();
        stats = transform.GetComponentsInChildren<EnemyStats>().ToList();

        houses.Clear();
        houses = transform.GetComponentsInChildren<House>().ToList();
    }

    public void On()
    {
        foreach(EnemyStats stat in stats)
        {
            stat.On();
        }
    }

    public void UpdStats(EnemyStats stat)
    { 
        if(eventPoint == null) return;

        stats.Remove(stat);

        if(stats.Count == 0) eventPoint.SetActive(false);
    }

    public void UpdHouses(House house)
    { 
        if(houses.Count == 0) return;

        houses.Remove(house);

        if(houses.Count == 0) Money.Plus(reward);
    }
}
