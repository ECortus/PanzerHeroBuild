using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

public class Level : MonoBehaviour
{
    [SerializeField] private Way way;
    public List<Vector3> GetWayPoints() => way.Points;

    public void On() => gameObject.SetActive(true);
    public void Off() => gameObject.SetActive(false);
    public void Eliminate() => Destroy(gameObject);

    public int moneyOnStart;

    public void StartLevel()
    {
        DataManager.Load();
        PrepareToStart();

        moneyOnStart = Statistics.Money;
    }

    public void PrepareToStart()
    {
        GameManager.Instance.SetActive(false);

        TankController.Instance.SpawnAtStart();
        PlayerStats.Instance.Off();

        UI.Instance.EnableStartUI();
    }
}
