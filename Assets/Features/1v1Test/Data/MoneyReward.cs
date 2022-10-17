using System.Collections;
using System.Collections.Generic;
using DataStructures.Variables;
using UnityEngine;

[CreateAssetMenu]
public class MoneyReward : ScriptableObject
{
    public FloatVariable sharedMoney;
    public FloatVariable initialMoneyCost;
    public FloatVariable scalingMoneyCost;
    public int totalStatCount;

    public float stageMoneyScale;
    public float intervalMoneyScale;
    public int stageExtraMoneyInterval;

    public void AddMoney(int stage)
    {
        sharedMoney.Set(sharedMoney.Get() + (scalingMoneyCost.Get() * stage + initialMoneyCost.Get()) * totalStatCount * stageMoneyScale);
        
        if (stage % stageExtraMoneyInterval == 0)
        {
            float m = stage + stageExtraMoneyInterval + (initialMoneyCost.Get() / scalingMoneyCost.Get()) - 1;
            float n = stage + (initialMoneyCost.Get() / scalingMoneyCost.Get());

            float finalValue = (Mathf.Pow(m, 2) - Mathf.Pow(n, 2) + m + n) / 2 * scalingMoneyCost.Get() * totalStatCount * intervalMoneyScale;
            Debug.LogWarning(finalValue);
            sharedMoney.Set(sharedMoney.Get() + finalValue);
        }
    }
}
