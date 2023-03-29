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

        SetupCell(damageCell, CostFormula(Statistics.DamageLVL));
        SetupCell(ArmorCell, CostFormula(Statistics.ArmorLVL));
        SetupCell(timeReloadCell, CostFormula(Statistics.TimeReloadLVL));

        UpdateUpCells();
    }

    public void UpDamage()
    {
        int lvl = Statistics.DamageLVL;
        Upgrade up = Modifications.UpgradeDamage;
        ModificationCell cell = damageCell;

        TryUp(lvl, up, cell);
    }

    public void UpArmor()
    {
        int lvl = Statistics.ArmorLVL;
        Upgrade up = Modifications.UpgradeArmor;
        ModificationCell cell = ArmorCell;

        TryUp(lvl, up, cell);
    }

    public void UpTimeReload()
    {
        int lvl = Statistics.TimeReloadLVL;
        Upgrade up = Modifications.UpgradeTimeReload;
        ModificationCell cell = timeReloadCell;

        TryUp(lvl, up, cell);
    }

    void SetupCell(ModificationCell cell, int cost)
    {
        cell.cost = cost;

        cell.UpdateCell();
    }

    void TryUp(int LVL, Upgrade UP, ModificationCell cell)
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
