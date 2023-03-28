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

    public static int DamageLVL
    {
        get
        {
            int lvl = PlayerPrefs.GetInt(DataManager.DamageKey, 0);
            return lvl;
        }

        set
        {
            int lvl = value;
            PlayerPrefs.SetInt(DataManager.DamageKey, lvl);
            PlayerPrefs.Save();

            int gunState = (lvl + 1) / 3;
            TankAppearanceUpgrade.Instance.SetGunState(gunState);

            int headState = lvl / 3;
            TankAppearanceUpgrade.Instance.SetHeadState(headState);
        }
    }
    
    public static int ArmorLVL
    {
        get
        {
            int lvl = PlayerPrefs.GetInt(DataManager.ArmorKey, 0);
            return lvl;
        }

        set
        {
            int lvl = value;
            PlayerPrefs.SetInt(DataManager.ArmorKey, lvl);
            PlayerPrefs.Save();

            int bodyState = lvl / 3;
            TankAppearanceUpgrade.Instance.SetBodyState(bodyState);
        }
    }

    public static int TimeReloadLVL
    {
        get
        {
            int lvl = PlayerPrefs.GetInt(DataManager.TimeReloadKey, 0);
            return lvl;
        }

        set
        {
            int lvl = value;
            PlayerPrefs.SetInt(DataManager.TimeReloadKey, lvl);
            PlayerPrefs.Save();

            int bodyState = lvl / 3;
            TankAppearanceUpgrade.Instance.SetAmmoState(bodyState);
        }
    }
}
