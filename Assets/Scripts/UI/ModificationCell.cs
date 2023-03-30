using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ModificationCell : MonoBehaviour
{
    [SerializeField] private CellType type;

    [HideInInspector] public int actualLVL
    {
        get
        {
            int value = -1;

            switch(type)
            {
                case CellType.Damage:
                    value = Statistics.DamageLVL;
                    break;
                case CellType.Armor:
                    value = Statistics.ArmorLVL;
                    break;
                case CellType.Reload:
                    value = Statistics.TimeReloadLVL;
                    break;
            }

            return value;
        }
    }

    public float currentChar
    {
        get
        {
            float value = -1f;

            switch(type)
            {
                case CellType.Damage:
                    value = Modifications.DamageMod;
                    value = System.MathF.Round(value, 0);
                    break;
                case CellType.Armor:
                    value = Modifications.ArmorMod;
                    value = System.MathF.Round(value, 0);
                    break;
                case CellType.Reload:
                    value = Modifications.TimeReloadMod;
                    value = System.MathF.Round(value, 2);
                    break;
            }

            return value;
        }
    }

    public float plus
    {
        get
        {
            float value = -1f;

            switch(type)
            {
                case CellType.Damage:
                    value = Modifications.GetDamagePlusChar(Statistics.DamageLVL);
                    value = System.MathF.Round(value, 0);
                    break;
                case CellType.Armor:
                    value = Modifications.GetArmorPlusChar(Statistics.ArmorLVL);
                    value = System.MathF.Round(value, 0);
                    break;
                case CellType.Reload:
                    value = Modifications.GetTimeReloadPlusChar(Statistics.TimeReloadLVL);
                    value = System.MathF.Round(value, 2);
                    break;
            }

            return value;
        }
    }
    [HideInInspector] public int cost;

    [Header("Info: ")]
    [SerializeField] private Image[] sliderImages;
    [SerializeField] private Sprite buyingImage, availableImage;
    [SerializeField] private TextMeshProUGUI levelTitle, currentLevel, nextLevel, currentCharText, plusCharText;
    

    [Header("Buy-button: ")]
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TMPro.TextMeshProUGUI costText;

    public void UpdateCell()
    {
        UpdateButton();
        UpdateSlider();
        UpdateText();
    }

    void UpdateSlider()
    {
        int lvl = actualLVL;

        int iter = lvl % Modifications.UpgradesInTier;
        if(lvl >= Modifications.MaxLevel) iter = 99;

        for(int i = 0; i < sliderImages.Length; i++)
        {
            if(i > iter - 1) sliderImages[i].sprite = availableImage;
            else sliderImages[i].sprite = buyingImage;
        }
    }

    void UpdateText()
    {
        int lvl = actualLVL;

        if(lvl < Modifications.MaxLevel)   
        {  
            levelTitle.text = $"LEVEL {lvl / Modifications.UpgradesInTier + 1}";
            currentLevel.text = $"{lvl  / Modifications.UpgradesInTier + 1}";
            nextLevel.text = $"{lvl / Modifications.UpgradesInTier + 2}";

            currentCharText.text = $"{currentChar}";
            if(plus > 0) plusCharText.text = $"+({plus})";
            else plusCharText.text = $"({plus})";
        }
        else
        {
            levelTitle.text = $"LEVEL {lvl / Modifications.UpgradesInTier + 1}";
            currentLevel.text = $"{lvl / Modifications.UpgradesInTier}";
            nextLevel.text = $"{lvl / Modifications.UpgradesInTier + 1}";

            currentCharText.text = $"{currentChar}";
            plusCharText.text = $"+--";
        }
    }

    void UpdateButton()
    {
        int lvl = actualLVL;

        if(lvl >= Modifications.MaxLevel)
        {
            image.sprite = ModificationStore.Instance.unavailableSpr;
            costText.text = "---";
            button.interactable = false;
            return;
        }

        if(cost <= Statistics.Money)
        {
            image.sprite = ModificationStore.Instance.availableSpr;
            button.interactable = true;
        }
        else
        {
            image.sprite = ModificationStore.Instance.unavailableSpr;
            button.interactable = false;
        }

        costText.text = $"{cost}";
    }
}

[System.Serializable]
public enum CellType
{
    Damage, Armor, Reload
}
