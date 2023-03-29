using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Modifications
{
    public static int MinLevel = 0;
    public static int MaxTier = 3;
    public static int UpgradesInTier = 3;
    public static int MaxLevel = UpgradesInTier * (MaxTier - 1);

    private static float baseArmor => PlayerStats.Instance.DefaultHP;
    private static float baseDamage => PlayerStats.Instance.DefaultDamage;
    private static float baseTimeReload => PlayerStats.Instance.DefaultTimeReload;

    private static float DamageStep = 7;
    private static float ArmorStep = 30f;
    private static float TimeReloadStep = -0.04f;

    delegate float Step(int lvl);

    static float Formula(float baseValue, int lvl, Step step)
    {
        float value = baseValue;

        for(int i = 0; i < lvl; i++)
        {
            value += step(i);
        }

        return value;
    }

    private static int DamageLVL { get { return Statistics.DamageLVL; } set { Statistics.DamageLVL = value; } }

    public static float DamageMod
    {
        get
        {
            Step step = GetDamagePlusChar;
            return Formula(baseDamage, DamageLVL, step);
        }
    }

    public static float GetDamagePlusChar(int lvl) 
    {
        return System.MathF.Round(DamageStep + DamageStep / (lvl + 1), 0);
    }

    public static void UpgradeDamage()
    {
        DamageLVL = DamageLVL + 1;
    }

    private static int ArmorLVL { get { return Statistics.ArmorLVL; } set { Statistics.ArmorLVL = value; } }

    public static float ArmorMod
    {
        get
        {
            Step step = GetArmorPlusChar;
            return Formula(baseArmor, ArmorLVL, step);
        }
    }

    public static float GetArmorPlusChar(int lvl) 
    {
        return System.MathF.Round(ArmorStep + ArmorStep / (lvl + 1), 0);
    }

    public static void UpgradeArmor()
    {
        ArmorLVL = ArmorLVL + 1;
    }

    private static int TimeReloadLVL { get { return Statistics.TimeReloadLVL; } set { Statistics.TimeReloadLVL = value; } }

    public static float TimeReloadMod
    {
        get
        {
            Step step = GetTimeReloadPlusChar;
            return Formula(baseTimeReload, TimeReloadLVL, step);
        }
    }

    public static float GetTimeReloadPlusChar(int lvl) 
    {
        return System.MathF.Round(TimeReloadStep + TimeReloadStep / (lvl + 1), 2);
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
