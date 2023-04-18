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

    bool on = false;

    void Start()
    {
        stats.Clear();
        stats = transform.GetComponentsInChildren<EnemyStats>().ToList();

        foreach(EnemyStats stat in stats)
        {
            stat.aggrAll = this;
        }

        houses.Clear();
        houses = transform.GetComponentsInChildren<House>().ToList();

        foreach(House house in houses)
        {
            house.zone = this;
        }
    }

    public void On()
    {
        if(on) return;

        on = true;
        foreach(EnemyStats stat in stats)
        {
            stat.On();
        }
    }

    public void UpdStats(EnemyStats stat)
    { 
        stats.Remove(stat);

        if(eventPoint == null) return;
        if(stats.Count == 0) eventPoint.SetActive(false);
    }

    public void UpdHouses(House house)
    { 
        if(houses.Count == 0) return;

        houses.Remove(house);

        if(houses.Count == 0) Reward();
    }

    void Reward()
    {
        Money.Plus(reward);
    }
}
