using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Modifications
{
    public static int MinLevel = 0;
    public static int MaxTier = 3;
    public static int UpgradesInTier = 3;
    public static int MaxLevel = UpgradesInTier * (MaxTier - 1);

    private static float baseValue = 1f;

    static float Formula(int lvl, float step)
    {
        return baseValue + (lvl * step);
    }

    private static float DamageStep = 0.1f;
    private static int DamageLVL { get { return Statistics.DamageLVL; } set { Statistics.DamageLVL = value; } }

    public static float DamageMod
    {
        get
        {
            int levelMod = DamageLVL;
            float step = DamageStep;

            return Formula(levelMod, step);
        }
    }

    public static float GetDamagePlusMod() 
    {
        return DamageStep * DamageLVL;
    }

    public static void UpgradeDamage()
    {
        DamageLVL = DamageLVL + 1;
    }

    private static float ArmorStep = 0.1f;
    private static int ArmorLVL { get { return Statistics.ArmorLVL; } set { Statistics.ArmorLVL = value; } }

    public static float ArmorMod
    {
        get
        {
            int levelMod = ArmorLVL;
            float step = ArmorStep;

            return Formula(levelMod, step);
        }
    }

    public static float GetArmorPlusMod() 
    {
        return ArmorStep * ArmorLVL;
    }

    public static void UpgradeArmor()
    {
        ArmorLVL = ArmorLVL + 1;
    }

    private static float TimeReloadStep = 0.1f;
    private static int TimeReloadLVL { get { return Statistics.TimeReloadLVL; } set { Statistics.TimeReloadLVL = value; } }

    public static float TimeReloadMod
    {
        get
        {
            int levelMod = TimeReloadLVL;
            float step = TimeReloadStep;

            return Formula(levelMod, step);
        }
    }

    public static float GetTimeReloadPlusMod() 
    {
        return TimeReloadStep * TimeReloadLVL;
    }

    public static void UpgradeTimeReload()
    {
        TimeReloadLVL = TimeReloadLVL + 1;
    }

    public static void Save()
    {
        PlayerPrefs.SetInt(DataManager.DamageKey, DamageLVL);
        PlayerPrefs.SetInt(DataManager.ArmorKey, ArmorLVL);
        PlayerPrefs.SetInt(DataManager.TimeReloadKey, TimeReloadLVL);
        PlayerPrefs.Save();
    }

    public static void Load()
    {
        DamageLVL = PlayerPrefs.GetInt(DataManager.DamageKey, 0);
        ArmorLVL = PlayerPrefs.GetInt(DataManager.ArmorKey, 0);
        TimeReloadLVL = PlayerPrefs.GetInt(DataManager.TimeReloadKey, 0);
    }
}
