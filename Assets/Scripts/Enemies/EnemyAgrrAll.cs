using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgrrAll : MonoBehaviour
{
    [SerializeField] private EnemyStats[] stats;

    public void On()
    {
        foreach(EnemyStats stat in stats)
        {
            stat.On();
        }
    }
}
