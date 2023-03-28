using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static string LevelIndexKey = "LevelIndex";

    public static string MoneyKey = "Money";

    public static string DamageKey = "Damage";
    public static string ArmorKey = "Health";
    public static string TimeReloadKey = "Reload";

    public static void Save()
    {
        Money.Save();
        Modifications.Save();
    }

    public static void Load()
    {
        Money.Load();
        Modifications.Load();
    }
}
