using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelTitleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelCounter;
    [SerializeField] private Image[] grid;

    [SerializeField] private Sprite complete, uncomplete;

    public void Upd()
    {
        levelCounter.text = $"LEVEL {Statistics.LevelIndex + 1}";

        int count = (Statistics.LevelIndex + 1) % grid.Length;
        foreach(Image child in grid)
        {
            if(count > 0)
            {
                child.sprite = complete;
                count--;
            }
            else
            {
                child.sprite = uncomplete;
            }
        }
    }
}
