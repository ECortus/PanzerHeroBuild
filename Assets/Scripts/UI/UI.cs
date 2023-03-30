using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI Instance { get; set; }

    [SerializeField] private GameObject infoUI, inputUI, startUI;
    [SerializeField] private EndGameUI endGameUI;

    [Space]
    [SerializeField] private LevelTitleUI levelTitleUI;
    [SerializeField] private ReloadingUI reloadingUI;

    void Awake()
    {
        Instance = this;
    }

    public void On()
    {
        /* infoUI.SetActive(true); */
        inputUI.SetActive(true);
    }

    public void Off()
    {
        /* infoUI.SetActive(false); */
        inputUI.SetActive(false);
    }

    public void StartPlay()
    {
        GameManager.Instance.SetActive(true);

        LevelManager.Instance.ActualLevel.moneyOnStart = Statistics.Money;
        
        PlayerStats.Instance.On();
        startUI.SetActive(false);
        On();
    }

    public void EnableStartUI()
    {
        Off();

        levelTitleUI.Upd();
        startUI.SetActive(true);
    }

    public void Restart()
    {   
        LevelManager.Instance.RestartLevel();
    }

    public void NextLevel()
    {
        LevelManager.Instance.NextLevel();
        endGameUI.Close();

        GameManager.Instance.SetActive(false);
        
        PlayerStats.Instance.Off();
        startUI.SetActive(true);
        Off();
    }

    public void EndLevel()
    {
        endGameUI.Open();
    }

    public void LoseLevel()
    {

    }

    public async UniTask Reload(int time)
    {
        await reloadingUI.StartReloading(time);
    }

    public void Aim()
    {
        ChangePlayType.Instance.Aim();
    }

    public void Ride()
    {
        ChangePlayType.Instance.Ride();
    }
}