using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TankAppearanceUpgrade : MonoBehaviour
{
    public static TankAppearanceUpgrade Instance { get; set; }

    [SerializeField] private int headState = -1;
    public void SetHeadState(int state)
    {
        headState = state;
        UpdateAppearance(headState, 2 + ammoState);
    }

    [SerializeField] private int gunState = -1;
    public void SetGunState(int state)
    {
        gunState = state;
        UpdateAppearance(gunState, 1);
    }

    [SerializeField] private int bodyState = -1;
    public void SetBodyState(int state)
    {
        bodyState = state;
        UpdateAppearance(bodyState, 0);
    }

    [SerializeField] private int ammoState = -1;
    public void SetAmmoState(int state)
    {
        ammoState = state;
        UpdateAppearance(headState, 2 + ammoState);
    }

    [Space]
    [SerializeField] private TankAppearance[] Upgrades = new TankAppearance[1];
    private int UpgradesCount => Upgrades.Length;

    void Awake() => Instance = this;

    void UpdateAppearance(int upgradeState, int index)
    {
        int UpgradeState = upgradeState;
        if(UpgradesCount == -1) return;

        if(UpgradeState > UpgradesCount - 1) UpgradeState = UpgradesCount - 1;
        else if (UpgradeState < 0) UpgradeState = 0;

        if(index + 1 > Upgrades[UpgradeState].Parts.Length) return;

        int state = UpgradeState;
        bool appearanceActive = false;

        for(int i = 0; i < UpgradesCount; i++)
        {
            if(i == state) appearanceActive = true;
            else appearanceActive = false;

            if(index > 1)
            {
                GameObject[] parts = Upgrades[i].Parts;
                for(int t = 2; t < 5; t++)
                {
                    if(t == index) parts[t].SetActive(appearanceActive);
                    else parts[t].SetActive(false);
                }
            }
            else
            {
                Upgrades[i].Parts[index].SetActive(appearanceActive);
            }
        }
    }
}

[System.Serializable]
public class TankAppearance
{
    public GameObject[] Parts = new GameObject[5];
}
