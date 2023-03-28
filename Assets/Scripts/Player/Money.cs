using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Money
{
    private static string key = DataManager.MoneyKey;
    private static int money { get { return Statistics.Money; } set { Statistics.Money = value; } }

    public static void Plus(int count) 
    {
        money += count;

        MoneyUI.Instance.UpdateMoney();
        ModificationStore.Instance.UpdateUpCells();
    }
    
    public static void Minus(int count)
    { 
        money -= count;
        if(money < 0) money = 0;

        MoneyUI.Instance.UpdateMoney();
        ModificationStore.Instance.UpdateUpCells();
    }

    public static void Save()
    {
        PlayerPrefs.SetInt(key, money);
        PlayerPrefs.Save();
    }
    
    public static void Load()
    {
        money = PlayerPrefs.GetInt(key, 0);
        MoneyUI.Instance.ResetMoney();
    }
}
