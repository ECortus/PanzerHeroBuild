using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPageUI : MonoBehaviour
{
    [SerializeField] private GameObject Mods;

    void Update()
    {
        if(Statistics.LevelIndex > 0)
        {
            Mods.SetActive(true);
            this.enabled = false;
        }
        else
        {
            Mods.SetActive(false);
        }
    }
}
