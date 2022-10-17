using System;
using System.Collections.Generic;
using DataStructures.Variables;
using UnityEngine;

[CreateAssetMenu]
public class ScalingStats : ScriptableObject
{
    public List<ScalingStatContainer> stats;
}

[Serializable]
public class ScalingStatContainer
{
    public FloatVariable stat;
    public float valueIncrease;
}
