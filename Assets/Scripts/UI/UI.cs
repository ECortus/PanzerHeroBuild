using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI Instance { get; set; }

    [SerializeField] private GameObject infoUI;
    [SerializeField] private GameObject inputUI;

    [Space]
    [SerializeField] private UIEvents Events;

    void Awake()
    {
        Instance = this;
    }

    public void On()
    {
        inputUI.SetActive(true);
    }

    public void Off()
    {
        inputUI.SetActive(false);
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
        Events.AimEvent.Invoke();
    }

    public void Ride()
    {
        Events.RideEvent.Invoke();
    }
}