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

    /* [SerializeField] private EnemyStats[] Enemies; */

    [HideInInspector] public int moneyOnStart;

    public void StartLevel()
    {
        DataManager.Load();
        PrepareToStart();

        /* foreach(EnemyStats go in Enemies)
        {
            go.OnGameObject();
        } */
    }

    public void EndLevel()
    {
		GameManager.Instance.SetActive(false);

        Money.Plus(500);
        MoneyUI.Instance.ResetMoney();
        Money.Save();

        UI.Instance.EndLevel();
    }

    public void PrepareToStart()
    {
        GameManager.Instance.SetActive(false);

        TankController.Instance.SpawnAtStart();
        PlayerStats.Instance.Off();

        UI.Instance.EnableStartUI();
    }
}
