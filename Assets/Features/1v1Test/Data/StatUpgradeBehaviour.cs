using DataStructures.Event;
using DataStructures.Variables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgradeBehaviour : MonoBehaviour
{
    public UpgradeValues upgradeValues;

    public Button button;
    public TMP_Text tmp;

    private int upgradeLevel;

    private void OnEnable()
    {
        upgradeValues.currency.RegisterOnValueChangedAction(SetInteractable);
        
        SetInteractable(upgradeValues.currency.Get());
        UpdateText();
    }

    private void OnDisable()
    {
        upgradeValues.currency.UnregisterOnValueChangedAction(SetInteractable);
    }

    private float GetUpgradePrice()
    {
        return upgradeValues.initialMoneyCost.Get() + upgradeLevel * upgradeValues.scalingMoneyCost.Get();
    }

    private void UpdateText()
    {
        string text = "";

        foreach (var upgrade in upgradeValues.upgrades)
        {
            text += upgrade.stat.name + " " + upgrade.stat.Get().ToString("+0;-#") + " ";
        }

        text += "Price: " + GetUpgradePrice();

        tmp.text = text;
    }
    
    private void SetInteractable(float currency)
    {
        float price = GetUpgradePrice();
        if (currency - price >= 0)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void Upgrade()
    {
        float price = GetUpgradePrice();
        if (!(upgradeValues.currency.Get() - price < 0))
        {
            upgradeValues.currency.Add(-price);
            foreach (var upgradeValue in upgradeValues.upgrades)
            {
                upgradeValue.stat.Add(upgradeValue.upgradeAmount);
            }

            upgradeLevel++;
            UpdateText();
        }
    }
}
