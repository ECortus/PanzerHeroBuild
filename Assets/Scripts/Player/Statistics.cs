using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Statistics
{
    public static int LevelIndex
    {
        get
        {
            int lvl = PlayerPrefs.GetInt(DataManager.LevelIndexKey, 0);
            return lvl;
        }

        set
        {
            int lvl = value;
            PlayerPrefs.SetInt(DataManager.LevelIndexKey, value);
            PlayerPrefs.Save();
        }
    }

    private static int _money;
    public static int Money { get { return _money; } set { _money = value; } }
}
