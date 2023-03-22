using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI Instance { get; set; }

    void Awake()
    {
        Instance = this;
    }

    public void On()
    {

    }

    public void Off()
    {

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
}