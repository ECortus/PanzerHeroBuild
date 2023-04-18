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
    private int level => Statistics.LevelIndex + 1;

    public void Upd()
    {
        levelCounter.text = $"LEVEL {level}";

        int count = (level) % grid.Length;
        count += ((level) / grid.Length > 0 && (level) % grid.Length == 0 ? grid.Length : 0);

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
