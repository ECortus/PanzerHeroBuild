using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI Instance { get; set; }

    [SerializeField] private GameObject infoUI, inputUI, startUI;

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
        
        PlayerStats.Instance.On();
        startUI.SetActive(false);
        On();
    }

    public void EnableStartUI()
    {
        Off();
        startUI.SetActive(true);
    }

    public void Restart()
    {

    }

    public void NextLevel()
    {

    }

    public void EndGame()
    {

    }

    public void LoseGame()
    {

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