using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ModificationCell : MonoBehaviour
{
    [HideInInspector] public int actualLVL = 0;
    [HideInInspector] public float currentChar = 0;
    [HideInInspector] public float plus = 0;
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
        int iter = actualLVL % Modifications.UpgradesInTier;

        if(actualLVL >= Modifications.MaxLevel) iter = 99;

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
            levelTitle.text = $"LEVEL {lvl / Modifications.UpgradesInTier}";
            currentLevel.text = $"{lvl  / Modifications.UpgradesInTier}";
            nextLevel.text = $"{lvl / Modifications.UpgradesInTier + 1}";

            currentCharText.text = $"99";
            plusCharText.text = $"+99";
        }
        else
        {
            levelTitle.text = $"LEVEL {lvl}";
            currentLevel.text = $"{actualLVL - 1}";
            nextLevel.text = $"{actualLVL}";

            currentCharText.text = $"{currentChar}";
            plusCharText.text = $"+{plus}";
        }
    }

    void UpdateButton()
    {
        if(actualLVL >= Modifications.MaxLevel)
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
