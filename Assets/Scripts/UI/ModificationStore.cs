using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModificationStore : MonoBehaviour
{
    public static ModificationStore Instance { get; set; }

    [SerializeField] private ModificationCell damageCell, ArmorCell, timeReloadCell;

    [Space]
    public Sprite availableSpr;
    public Sprite unavailableSpr;

    [Space]
    [SerializeField] private int zeroModCost = 35;
    [SerializeField] private int costStep = 25;

    delegate void Upgrade();

    void Awake() => Instance = this;

    void Start()
    {
        DataManager.Load();

        SetupCell(damageCell, Statistics.DamageLVL, Modifications.GetDamagePlusMod(), CostFormula(Statistics.DamageLVL));
        SetupCell(ArmorCell, Statistics.ArmorLVL, Modifications.GetArmorPlusMod(), CostFormula(Statistics.ArmorLVL));
        SetupCell(timeReloadCell, Statistics.TimeReloadLVL, Modifications.GetTimeReloadPlusMod(), CostFormula(Statistics.TimeReloadLVL));

        UpdateUpCells();
    }

    public void UpDamage()
    {
        int lvl = Statistics.DamageLVL;
        Upgrade up = Modifications.UpgradeDamage;
        float cur = Modifications.ArmorMod;
        float plus = Modifications.GetDamagePlusMod();
        ModificationCell cell = damageCell;

        TryUp(lvl, up, cur, plus, cell);
    }

    public void UpArmor()
    {
        int lvl = Statistics.ArmorLVL;
        Upgrade up = Modifications.UpgradeArmor;
        float cur = Modifications.ArmorMod;
        float plus = Modifications.GetArmorPlusMod();
        ModificationCell cell = ArmorCell;

        TryUp(lvl, up, cur, plus, cell);
    }

    public void UpTimeReload()
    {
        int lvl = Statistics.TimeReloadLVL;
        Upgrade up = Modifications.UpgradeTimeReload;
        float cur = Modifications.ArmorMod;
        float plus = Modifications.GetTimeReloadPlusMod();
        ModificationCell cell = timeReloadCell;

        TryUp(lvl, up, cur, plus, cell);
    }

    void SetupCell(ModificationCell cell, int LVL, float plus, int cost)
    {
        cell.actualLVL = LVL;
        cell.plus = plus;
        cell.cost = cost;

        cell.UpdateCell();
    }

    void TryUp(int LVL, Upgrade UP, float current, float plus, ModificationCell cell)
    {
        if(LVL >= Modifications.MaxLevel)
        {
            return;
        }

        int money = Statistics.Money;
        int cost = CostFormula(LVL);

        if(money >= cost)
        {
            UP();
            Money.Minus(cost);
            Money.Save();

            cell.actualLVL = LVL + 1;
            cell.currentChar = current;
            cell.plus = plus;
            cell.cost = CostFormula(LVL + 1);
            cell.UpdateCell();
        }
    }

    int CostFormula(int lvl)
    {
        return zeroModCost + lvl * costStep;
    }

    public void UpdateUpCells()
    {
        damageCell.UpdateCell();
        ArmorCell.UpdateCell();
        timeReloadCell.UpdateCell();
    }
}
