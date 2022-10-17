using System.Collections.Generic;
using DataStructures.Variables;
using UnityEngine;

[CreateAssetMenu]
public class UpgradeValues : ScriptableObject
{
    public FloatVariable currency;
    public FloatVariable initialMoneyCost;
    public FloatVariable scalingMoneyCost;
    
    public List<UpgradeValuesContainer> upgrades;
}

[System.Serializable]
public struct UpgradeValuesContainer
{
    public Stat stat;
    public float upgradeAmount;
}
