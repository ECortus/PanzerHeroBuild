using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TankAppearanceUpgrade : MonoBehaviour
{
    [Range(0, 2)]
    public int UpgradeState = 0;
    public void SetState(int state)
    {
        UpgradeState = state;
        UpdateAppearance();
    }

    [Space]
    [SerializeField] private TankAppearance[] Upgrades = new TankAppearance[1];
    private int UpgradesCount => Upgrades.Length;

    void Update()
    {
        UpdateAppearance();
    }

    void UpdateAppearance()
    {
        if(UpgradesCount == -1) return;

        if(UpgradeState > UpgradesCount) UpgradeState = UpgradesCount - 1;
        else if (UpgradeState < 0) UpgradeState = 0;

        int state = UpgradeState;
        bool appearanceActive = false;

        for(int i = 0; i < UpgradesCount; i++)
        {
            if(i == state) appearanceActive = true;
            else appearanceActive = false;

            GameObject[] parts = Upgrades[i].Parts;
            foreach(GameObject part in parts)
            {
                part.SetActive(appearanceActive);
            }
        }
    }
}

[System.Serializable]
public class TankAppearance
{
    public GameObject[] Parts = new GameObject[3];
}
